using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProfileService
{
    public UserProfile ActiveProfile { get; private set; }
    public IReadOnlyList<UserProfile> AllProfiles => _profiles.AsReadOnly();

    public event Action<UserProfile> ActiveProfileChanged;
    public event Action<IReadOnlyList<UserProfile>> ProfilesListChanged;
    public event Action ProfilesLoaded;

    private readonly List<UserProfile> _profiles = new List<UserProfile>();
    private string ProfilesFilePath => System.IO.Path.Combine(Application.persistentDataPath, "profiles.json");

    public void LoadProfiles()
    {
        _profiles.Clear();
        ActiveProfile = null;

        if (!System.IO.File.Exists(ProfilesFilePath))
        {
            ProfilesLoaded?.Invoke();
            ProfilesListChanged?.Invoke(AllProfiles);
            return;
        }

        string json = System.IO.File.ReadAllText(ProfilesFilePath);
        var data = JsonUtility.FromJson<UserProfilesData>(json);

        if (data == null)
        {
            ProfilesLoaded?.Invoke();
            ProfilesListChanged?.Invoke(AllProfiles);
            return;
        }

        _profiles.AddRange(data.Profiles);

        if (!string.IsNullOrEmpty(data.ActiveProfileId))
            ActiveProfile = _profiles.FirstOrDefault(p => p.ProfileId == data.ActiveProfileId);

        ProfilesLoaded?.Invoke();
        ProfilesListChanged?.Invoke(AllProfiles);
        ActiveProfileChanged?.Invoke(ActiveProfile);
    }

    public UserProfile CreateProfile(string playerName, string avatarId = null)
    {
        var profile = new UserProfile(playerName, avatarId);
        _profiles.Add(profile);

        SetActiveProfile(profile.ProfileId);

        SaveProfiles();
        ProfilesListChanged?.Invoke(AllProfiles);
        return profile;
    }

    public void SaveProfiles()
    {
        var data = new UserProfilesData
        {
            Profiles = _profiles,
            ActiveProfileId = ActiveProfile?.ProfileId
        };

        string json = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(ProfilesFilePath, json);
    }

    public void DeleteProfile(string profileId)
    {
        var profile = _profiles.FirstOrDefault(p => p.ProfileId == profileId);
        if (profile == null) return;

        _profiles.Remove(profile);

        if (ActiveProfile == profile)
            ActiveProfile = _profiles.FirstOrDefault();

        SaveProfiles();
        ProfilesListChanged?.Invoke(AllProfiles);
        ActiveProfileChanged?.Invoke(ActiveProfile);
    }

    public void SetActiveProfile(string profileId)
    {
        var profile = _profiles.FirstOrDefault(p => p.ProfileId == profileId);
        if (profile == null) return;

        ActiveProfile = profile;
        SaveProfiles();
        ActiveProfileChanged?.Invoke(ActiveProfile);
    }
}
