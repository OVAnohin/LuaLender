using System;
using UnityEngine;
using UnityEngine.UI;

public class ProfileMenuUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button createNewProfileButton;
    //[SerializeField] private Button selectButton;
    //[SerializeField] private Button deleteButton;
    [SerializeField] private CanvasGroup canvasGroup;

    private ProfileMenuController _profileMenuController;

    internal void Initialize(ProfileMenuController profileMenuController)
    {
        _profileMenuController = profileMenuController;

        closeButton.onClick.AddListener(_profileMenuController.OnCloseProfileMenuClicked);
        createNewProfileButton.onClick.AddListener(_profileMenuController.OnCreateNewProfileClicked);

        Subscribe();
    }

    private void Subscribe()
    {
        _profileMenuController.ShowWindow += ShowWindow;
        _profileMenuController.HideWindow += HideWindow;
    }

    public void Deinitialize()
    {
        if (_profileMenuController == null)
            return;

        _profileMenuController.ShowWindow -= ShowWindow;
        _profileMenuController.HideWindow -= HideWindow;
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
