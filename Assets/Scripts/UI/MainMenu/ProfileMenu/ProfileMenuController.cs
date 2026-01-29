using System;
using UnityEngine;

public class ProfileMenuController : MonoBehaviour
{
    [SerializeField] private ProfileMenuUI profileMenuUI;
    [SerializeField] private MainMenuController mainMenuController;

    public event EventHandler CreateNewProfileMenuClicked;
    public event EventHandler SelectProfileMenuClicked;
    public event EventHandler ShowWindow;
    public event EventHandler HideWindow;

    private void Awake()
    {
        if (profileMenuUI.gameObject.activeSelf == false)
            profileMenuUI.gameObject.SetActive(true);

        profileMenuUI.Initialize(this);
    }

    private void OnEnable()
    {
        mainMenuController.ProfileMenuClicked += OnProfileMenuClicked;
    }

    private void OnDisable()
    {
        mainMenuController.ProfileMenuClicked -= OnProfileMenuClicked;
    }

    private void OnProfileMenuClicked(object sender, EventArgs e)
    {
        ShowWindow?.Invoke(this, EventArgs.Empty);
    }

    internal void OnCloseProfileMenuClicked()
    {
        HideWindow?.Invoke(this, EventArgs.Empty);
    }

    internal void OnCreateNewProfileClicked()
    {
        CreateNewProfileMenuClicked?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        profileMenuUI.Deinitialize();
    }

    internal void OnSelectProfileMenuClicked()
    {
        SelectProfileMenuClicked?.Invoke(this, EventArgs.Empty);
    }
}

