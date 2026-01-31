using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectProfileMenuController : MonoBehaviour
{
    [SerializeField] private SelectProfileMenuUI selectProfileMenuUI;
    [SerializeField] private ProfileMenuController profileMenuController;

    public event EventHandler ShowWindow;
    public event EventHandler HideWindow;
    public event EventHandler<List<ProfileMenuEventArgs>> CurrentProfileListUpdated;

    private ProfileService _profileService;
    private int _selectedIndex;
    private int[] _currentIndexes;
    private int _numberSelectableProfiles;

    private void Awake()
    {
        _profileService = AppBootstrap.Instance.ProfileService;

        if (selectProfileMenuUI.gameObject.activeSelf == false)
            selectProfileMenuUI.gameObject.SetActive(true);

        selectProfileMenuUI.Initialize(this);

        _numberSelectableProfiles = selectProfileMenuUI.NumberOfProfiles;
        _selectedIndex = selectProfileMenuUI.SectctedIndex;
        _currentIndexes = new int[_numberSelectableProfiles];

        InitCurrentIndexes();
    }

    private void OnEnable()
    {
        _profileService.ActiveProfileChanged += OnActiveProfileChanged;
        _profileService.ProfilesListChanged += OnProfilesListChanged;
        _profileService.ProfilesLoaded += OnProfilesLloaded;

        profileMenuController.SelectProfileMenuClicked += SelectProfileMenuClicked;
    }

    private void OnDisable()
    {
        _profileService.ActiveProfileChanged -= OnActiveProfileChanged;
        _profileService.ProfilesListChanged -= OnProfilesListChanged;
        _profileService.ProfilesLoaded -= OnProfilesLloaded;

        profileMenuController.SelectProfileMenuClicked -= SelectProfileMenuClicked;
    }

    private void InitCurrentIndexes()
    {
        for (int i = 0; i < _numberSelectableProfiles; i++)
            _currentIndexes[i] = i;

        IReadOnlyList<UserProfile> profiles = _profileService.AllProfiles;
        UserProfile profile = _profileService.ActiveProfile;

        for (int i = 0; i < profiles.Count; i++)
        {
            if (profiles[i].ProfileId.Equals(profile.ProfileId))
            {
                _selectedIndex = i;
                break;
            }
        }
    }

    private void OnProfilesLloaded()
    {
        Debug.Log("Profiles Loaded");
    }

    private void OnProfilesListChanged(IReadOnlyList<UserProfile> list)
    {
        FillOutProfileMenu();
    }

    private void OnActiveProfileChanged(UserProfile profile)
    {
        FillOutProfileMenu();
    }

    private void SelectProfileMenuClicked(object sender, EventArgs e)
    {
        FillOutProfileMenu();
        ShowWindow?.Invoke(this, EventArgs.Empty);
    }

    internal void OnCloseClicked()
    {
        HideWindow?.Invoke(this, EventArgs.Empty);
    }

    public void OnOkClicked()
    {
        Debug.Log("OnOkClicked");
        HideWindow?.Invoke(this, EventArgs.Empty);

        //ProfileService profileService = AppBootstrap.Instance.ProfileService;
        //profileService.CreateProfile(text);
    }

    private void OnDestroy()
    {
        selectProfileMenuUI.Deinitialize();
    }

    internal void OnUpClicked()
    {
        IReadOnlyList<UserProfile> profiles = _profileService.AllProfiles;

        if (profiles.Count <= _numberSelectableProfiles)
            return;

        if (_currentIndexes[0] == 0)
            return;

        for (int i = 0; i < _numberSelectableProfiles; i++)
            _currentIndexes[i] = _currentIndexes[i] - 1;

        FillOutProfileMenu();
    }

    internal void OnDownClicked()
    {
        IReadOnlyList<UserProfile> profiles = _profileService.AllProfiles;

        if (profiles.Count <= _numberSelectableProfiles)
            return;

        if (_currentIndexes[_currentIndexes.Length - 1] == profiles.Count - 1)
            return;

        for (int i = 0; i < _numberSelectableProfiles; i++)
            _currentIndexes[i] = _currentIndexes[i] + 1;

        FillOutProfileMenu();
    }

    private void FillOutProfileMenu()
    {
        IReadOnlyList<UserProfile> profiles = _profileService.AllProfiles;
        List<ProfileMenuEventArgs> profileList = new List<ProfileMenuEventArgs>();
        UserProfile profile = _profileService.ActiveProfile;

        for (int i = 0; i < _numberSelectableProfiles; i++)
            profileList.Add(new ProfileMenuEventArgs("Zero", "No Profile", i));

        if (profiles != null)
        {
            for (int i = 0; i < _currentIndexes.Length; i++)
                profileList[i] = new ProfileMenuEventArgs(profiles[_currentIndexes[i]].ProfileId, profiles[_currentIndexes[i]].PlayerInfo.PlayerName, i);
        }

        CurrentProfileListUpdated?.Invoke(this, profileList);
    }
}
