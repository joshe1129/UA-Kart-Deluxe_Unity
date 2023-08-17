using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIConfirmationController : MonoBehaviour
{
    public static UIConfirmationController Instance { get; private set; }

    [SerializeField]
    private Button cancelButton;
    [SerializeField]
    private Button confirmButton;
    [SerializeField]
    private TMP_Text confirmationLabel;

    private Animator animator;

    private UnityAction cancelCallback;
    private UnityAction confirmCallback;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        cancelButton.onClick.AddListener(CancelButton);
        confirmButton.onClick.AddListener(ConfirmButton);
    }

    public void ShowConfirmationPanel(string label, UnityAction cancelCallback, UnityAction confirmAction)
    {
        animator.SetTrigger("show");
        confirmationLabel.text = label;
        this.cancelCallback = cancelCallback;
        this.confirmCallback = confirmAction;
    }

    private void ConfirmButton()
    {
        if (confirmCallback != null)
        {
            confirmCallback();
            confirmCallback = null;
        }
        animator.SetTrigger("hide");
    }

    private void CancelButton()
    {
        if (cancelCallback != null)
        {
            cancelCallback();
            cancelCallback = null;
        }
        animator.SetTrigger("hide");
    }
}
