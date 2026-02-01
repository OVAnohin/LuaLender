using System;
using UnityEngine;

public class CreateProfileMenuController : MonoBehaviour
{
    [SerializeField] private CreateProfileMenuUI createProfileMenuUI;
    [SerializeField] private ProfileMenuController profileMenuController;

    public event EventHandler ShowWindow;
    public event EventHandler HideWindow;

    private void Awake()
    {
        createProfileMenuUI.Initialize(this);
    }

    private void OnEnable()
    {
        profileMenuController.CreateNewProfileMenuClicked += OnCreateNewProfileMenuClicked;
    }

    private void OnDisable()
    {
        profileMenuController.CreateNewProfileMenuClicked -= OnCreateNewProfileMenuClicked;
    }

    private void OnCreateNewProfileMenuClicked(object sender, EventArgs e)
    {
        ShowWindow?.Invoke(this, EventArgs.Empty);
    }

    internal void OnCloseCreateProfileMenuClicked()
    {
        HideWindow?.Invoke(this, EventArgs.Empty);
    }

    internal void OnOkClicked(string text)
    {
        HideWindow?.Invoke(this, EventArgs.Empty);

        ProfileService profileService = AppBootstrap.Instance.ProfileService;
        UserProfile newProfile =  profileService.CreateProfile(text);
        profileService.SetActiveProfile(newProfile.ProfileId);

        profileMenuController.OnCloseProfileMenuClicked();
    }

    private void OnDestroy()
    {
        createProfileMenuUI.Deinitialize();
    }
}
