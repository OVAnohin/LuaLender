using System;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private MainMenuUI mainMenuUI;

    public event EventHandler ProfileMenuClicked;
    public event Action<bool> UpdatePlayButtonStatus;
    public event Action<string> UpdateUserName;
    //public event EventHandler SettingsMenuClicked;

    private ProfileService _profileService;
    private AppFlowController _appFlow;

    private void Awake()
    {
        _profileService = AppBootstrap.Instance.ProfileService;
        _appFlow = AppBootstrap.Instance.AppFlow;

        mainMenuUI.Initialize(this);
    }

    private void OnEnable()
    {
        _profileService.ActiveProfileChanged += OnActiveProfileChanged;
        _profileService.ProfilesListChanged += OnProfilesListChanged;

        UpdateMainMenuData();
    }

    private void OnDisable()
    {
        // Отписка
        _profileService.ActiveProfileChanged -= OnActiveProfileChanged;
        _profileService.ProfilesListChanged -= OnProfilesListChanged;
    }

    public void OnPlayClicked()
    {
        _appFlow.SetState(AppState.Gameplay);
    }

    public void OnProfileClicked()
    {
        ProfileMenuClicked?.Invoke(this, EventArgs.Empty);
    }

    public void OnSettingsClicked()
    {
        // открыть UI настроек
        Debug.Log("Open Settings Menu");
    }

    public void OnQuitClicked()
    {
        _appFlow.Exit();
    }

    private void OnActiveProfileChanged(UserProfile profile)
    {
        UpdateMainMenuData();
    }

    private void OnProfilesListChanged(IReadOnlyList<UserProfile> profiles)
    {
        UpdateMainMenuData();
    }

    private void UpdateMainMenuData()
    {
        UpdatePlayButtonStatus?.Invoke(_profileService.ActiveProfile != null);

        string userName = "No User";
        if (_profileService.ActiveProfile != null)
            userName = _profileService.ActiveProfile.PlayerInfo.PlayerName;

        UpdateUserName?.Invoke(userName);
    }

    private void OnDestroy()
    {
        mainMenuUI.Deinitialize();
    }
}
