using System;
using UnityEngine;

public class DeleteProfileMenuController : MonoBehaviour
{
    [SerializeField] private DeleteProfileMenuUI deleteProfileMenuUI;
    [SerializeField] private ProfileMenuController profileMenuController;

    public event EventHandler<string> ShowWindow;
    public event EventHandler HideWindow;

    private void Awake()
    {
        deleteProfileMenuUI.Initialize(this);
    }

    private void OnEnable()
    {
        profileMenuController.DeleteProfileMenuClicked += DeleteProfileMenuClicked;
    }

    private void OnDisable()
    {
        profileMenuController.DeleteProfileMenuClicked -= DeleteProfileMenuClicked;
    }

    private void DeleteProfileMenuClicked(object sender, EventArgs e)
    {
        ProfileService profileService = AppBootstrap.Instance.ProfileService;

        if (profileService.ActiveProfile == null)
            return;

        ShowWindow?.Invoke(this, profileService.ActiveProfile.PlayerInfo.PlayerName);
    }

    internal void OnCloseCreateProfileMenuClicked()
    {
        HideWindow?.Invoke(this, EventArgs.Empty);
    }

    internal void OnOkClicked()
    {
        HideWindow?.Invoke(this, EventArgs.Empty);

        ProfileService profileService = AppBootstrap.Instance.ProfileService;
        string profileID = profileService.ActiveProfile.ProfileId;

        if (!System.String.IsNullOrEmpty(profileID))
            profileService.DeleteProfile(profileID);
    }

    private void OnDestroy()
    {
        deleteProfileMenuUI.Deinitialize();
    }
}
