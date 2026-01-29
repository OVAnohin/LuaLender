using System;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private MainMenuUI mainMenuUI;

    public event EventHandler ProfileMenuClicked;
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
        // Подписка на события ProfileService
        _profileService.ActiveProfileChanged += OnActiveProfileChanged;
        _profileService.ProfilesListChanged += OnProfilesListChanged;

        // Инициализация кнопки Play
        UpdatePlayButtonState();
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
        Debug.Log("OnProfileClicked");
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
        UpdatePlayButtonState();
    }

    private void OnProfilesListChanged(IReadOnlyList<UserProfile> profiles)
    {
        UpdatePlayButtonState();
    }

    private void UpdatePlayButtonState()
    {
        mainMenuUI.UpdatePlayButtonState(_profileService.ActiveProfile != null);
    }
}
