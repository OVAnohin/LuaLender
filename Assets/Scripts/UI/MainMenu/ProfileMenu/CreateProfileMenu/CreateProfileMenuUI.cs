using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateProfileMenuUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button okButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private CanvasGroup canvasGroup;

    public string PlayerName => nameInput.text;

    private CreateProfileMenuController _createProfileMenuController;

    internal void Initialize(CreateProfileMenuController createProfileMenuController)
    {
        _createProfileMenuController = createProfileMenuController;

        okButton.onClick.AddListener(OnOkClicked);
        closeButton.onClick.AddListener(_createProfileMenuController.OnCloseCreateProfileMenuClicked);

        Subscribe();
    }

    private void OnOkClicked()
    {
        _createProfileMenuController.OnOkClicked(nameInput.text);
    }

    private void Subscribe()
    {
        _createProfileMenuController.ShowWindow += ShowWindow;
        _createProfileMenuController.HideWindow += HideWindow;
    }

    public void Deinitialize()
    {
        if (_createProfileMenuController == null)
            return;

        _createProfileMenuController.ShowWindow -= ShowWindow;
        _createProfileMenuController.HideWindow -= HideWindow;
    }

    private void Start()
    {
        HideWindow(null, null);
    }

    private void ShowWindow(object sender, System.EventArgs e)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void HideWindow(object sender, System.EventArgs e)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
