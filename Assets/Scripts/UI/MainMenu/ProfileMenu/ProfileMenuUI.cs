using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ProfileMenuUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button createNewProfileButton;
    [SerializeField] private Button selectButton;
    [SerializeField] private Button deleteButton;

    private ProfileMenuController _profileMenuController;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    internal void Initialize(ProfileMenuController profileMenuController)
    {
        _profileMenuController = profileMenuController;

        closeButton.onClick.AddListener(_profileMenuController.OnCloseProfileMenuClicked);
        selectButton.onClick.AddListener(_profileMenuController.OnSelectProfileMenuClicked);
        createNewProfileButton.onClick.AddListener(_profileMenuController.OnCreateNewProfileClicked);
        deleteButton.onClick.AddListener(_profileMenuController.OnDeleteProfileClicked);

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
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    private void HideWindow(object sender, System.EventArgs e)
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
}
