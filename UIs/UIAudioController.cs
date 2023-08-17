using UnityEngine;
using UnityEngine.UI;

public class UIAudioController : MonoBehaviour
{
    [SerializeField]
    private Animator mainCanvasAnimator;

    [SerializeField]
    private Slider globalVolume;
    [SerializeField]
    private Slider musicVolume;
    [SerializeField]
    private Slider soundEffectsVolume;
    [SerializeField]
    private Slider environmentVolume;

    private UIEventsTrigger uiEventsTrigger;

    private bool isDirty;
    private float savedGlobalVolume;
    private float savedMusicVolume;
    private float savedSoundEffectsVolume;
    private float savedEnvironmentVolume;

    private void Awake()
    {
        uiEventsTrigger = GetComponent<UIEventsTrigger>();

        AudioSettingsData saveData = AudioSettingsSaveDataObj.data;

        globalVolume.value = saveData.globalVolume;
        savedGlobalVolume = saveData.globalVolume;

        musicVolume.value = saveData.musicVolume;
        savedMusicVolume = saveData.musicVolume;

        soundEffectsVolume.value = saveData.soundEffectsVolume;
        savedSoundEffectsVolume = saveData.soundEffectsVolume;

        environmentVolume.value = saveData.environmentVolume;
        savedEnvironmentVolume = saveData.environmentVolume;

        globalVolume.onValueChanged.AddListener(DirtyAudioSettingsValues);
        musicVolume.onValueChanged.AddListener(DirtyAudioSettingsValues);
        soundEffectsVolume.onValueChanged.AddListener(DirtyAudioSettingsValues);
        environmentVolume.onValueChanged.AddListener(DirtyAudioSettingsValues);
    }

    private void Start()
    {
        uiEventsTrigger.onSubmitEvent += Save;
        uiEventsTrigger.onCancelEvent += Cancel;
    }

    public void SetGlobalVolume(float volume)
    {
        AudioSettingsController.Instance.SetGlobalVolume(volume);
    }

    public void SetMusicVolume(float volume)
    {
        AudioSettingsController.Instance.SetMusicVolume(volume);
    }

    public void SetSoundEffectsVolume(float volume)
    {
        AudioSettingsController.Instance.SetSoundEffectsVolume(volume);
    }

    public void SetEnvironmentVolume(float volume)
    {
        AudioSettingsController.Instance.SetEnvironmentVolume(volume);
    }

    private void Cancel()
    {
        if (isDirty)
        {
            UIConfirmationController.Instance.ShowConfirmationPanel("Do you want to save?", Recover, Save);
        }
        else
        {
            mainCanvasAnimator.SetTrigger("Settings");
        }
    }

    private void DirtyAudioSettingsValues(float newVolume)
    {
        if (!isDirty)
        {
            isDirty = true;
        }
    }

    private void Save()
    {
        isDirty = false;
        savedGlobalVolume = AudioSettingsSaveDataObj.data.globalVolume;
        savedMusicVolume = AudioSettingsSaveDataObj.data.musicVolume;
        savedSoundEffectsVolume = AudioSettingsSaveDataObj.data.soundEffectsVolume;
        savedEnvironmentVolume = AudioSettingsSaveDataObj.data.environmentVolume;
        SaveDataController.Instance.SaveData();

        mainCanvasAnimator.SetTrigger("Settings");
    }

    private void Recover()
    {
        isDirty = false;
        SetGlobalVolume(savedGlobalVolume);
        globalVolume.value = savedGlobalVolume;

        SetMusicVolume(savedMusicVolume);
        musicVolume.value = savedMusicVolume;

        SetSoundEffectsVolume(savedSoundEffectsVolume);
        soundEffectsVolume.value = savedSoundEffectsVolume;

        SetEnvironmentVolume(savedEnvironmentVolume);
        environmentVolume.value = savedEnvironmentVolume;

        mainCanvasAnimator.SetTrigger("Settings");
    }
}
