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

    private CreateProfileMenuController _createProfileMenuController;

    internal void Initialize(CreateProfileMenuController createProfileMenuController)
    {
        _createProfileMenuController = createProfileMenuController;
        okButton.onClick.AddListener(_createProfileMenuController.OnOkClicked);
        closeButton.onClick.AddListener(_createProfileMenuController.OnCloseCreateProfileMenuClicked);

        Subscribe();
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

    private void OnCreateClicked()
    {
        string playerName = nameInput.text;
        //_controller.OnCreateProfileClicked(playerName);
    }

    public void ClearInput()
    {
        nameInput.text = string.Empty;
    }
}
