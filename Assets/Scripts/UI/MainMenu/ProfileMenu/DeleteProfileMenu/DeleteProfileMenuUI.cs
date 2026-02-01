using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class DeleteProfileMenuUI : MonoBehaviour
{
    [SerializeField] private TMP_Text userName;
    [SerializeField] private Button okButton;
    [SerializeField] private Button closeButton;

    private DeleteProfileMenuController _deleteProfileMenuController;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    internal void Initialize(DeleteProfileMenuController deleteProfileMenuController)
    {
        _deleteProfileMenuController = deleteProfileMenuController;

        okButton.onClick.AddListener(OnOkClicked);
        closeButton.onClick.AddListener(_deleteProfileMenuController.OnCloseCreateProfileMenuClicked);

        Subscribe();
    }

    private void OnOkClicked()
    {
        _deleteProfileMenuController.OnOkClicked();
    }

    private void Subscribe()
    {
        _deleteProfileMenuController.ShowWindow += ShowWindow;
        _deleteProfileMenuController.HideWindow += HideWindow;
    }

    private void ShowWindow(object sender, string playerName)
    {
        userName.text = playerName;

        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public void Deinitialize()
    {
        if (_deleteProfileMenuController == null)
            return;

        _deleteProfileMenuController.ShowWindow -= ShowWindow;
        _deleteProfileMenuController.HideWindow -= HideWindow;
    }

    private void Start()
    {
        HideWindow(null, null);
    }

    private void HideWindow(object sender, System.EventArgs e)
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
}
