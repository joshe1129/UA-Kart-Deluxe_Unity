using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class UICameraController : MonoBehaviour
{
    [SerializeField] Animator mainCanvasAnimator;
    [SerializeField] Slider fovSlider, distanceSlider, heightSlider, angleSlider;
    [SerializeField] Toggle resetToggle;

    private bool isDirty;
    private UIEventsTrigger uiEventsTrigger;

    private void Awake()
    {
        uiEventsTrigger = GetComponent<UIEventsTrigger>();
        //Adding the listeners to the sliders
        fovSlider.onValueChanged.AddListener((value) => { CameraSettingsController.Instance.ChangeFOVValue(value); ChangeUIValues(value, fovSlider); DirtyCameraSettingsValues(); });
        distanceSlider.onValueChanged.AddListener((value) => { CameraSettingsController.Instance.ChangeDistanceValue(value); ChangeUIValues(value, distanceSlider); DirtyCameraSettingsValues(); });
        heightSlider.onValueChanged.AddListener((value) => { CameraSettingsController.Instance.ChangeHeightValue(value); ChangeUIValues(value, heightSlider); DirtyCameraSettingsValues(); });
        angleSlider.onValueChanged.AddListener((value) => { CameraSettingsController.Instance.ChangeAngleValue(value); ChangeUIValues(value, angleSlider); DirtyCameraSettingsValues(); });
        resetToggle.onValueChanged.AddListener((value) => { CameraSettingsController.Instance.ResetValues(); SetValues(); DirtyCameraSettingsValues(); });
        uiEventsTrigger.onSubmitEvent += Save;
        uiEventsTrigger.onCancelEvent += Cancel;
    }

    private void OnEnable()
    {
        SetValues();  
    }

    //Setter of the values
    void SetValues()
    {
        ChangeUIValues(CameraSettingsSaveDataObj.data.fovCamera, fovSlider);
        ChangeUIValues(CameraSettingsSaveDataObj.data.distanceCamera, distanceSlider);
        ChangeUIValues(CameraSettingsSaveDataObj.data.heightCamera, heightSlider);
        ChangeUIValues(CameraSettingsSaveDataObj.data.angleCamera, angleSlider);
        resetToggle.isOn = false;
    }

    //Setter for the text
    void SetSliderText(float value, Slider sliderObject)
    {
        sliderObject.transform.parent.GetChild(0).GetComponent<TMP_Text>().text = (Math.Truncate(value * 100) / 100).ToString();
    }

    //Changes the values of the sliders and calls to change the texts
    void ChangeUIValues(float value, Slider sliderObject)
    {
        sliderObject.value = value;
        SetSliderText(value, sliderObject);
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

    private void DirtyCameraSettingsValues()
    {
        if (!isDirty)
        {
            isDirty = true;
        }
    }

    private void Save()
    {
        isDirty = false;
        CameraSettingsController.Instance.SaveData();
        mainCanvasAnimator.SetTrigger("Settings");
    }

    private void Recover()
    {
        isDirty = false;
        mainCanvasAnimator.SetTrigger("Settings");
    }
}
