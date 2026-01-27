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

    public void LoadProfiles()
    {
        // Здесь можно загрузку из PlayerPrefs, JSON, файл и т.д.
        // Сейчас заглушка: имитация пустого хранилища
        _profiles.Clear();

        // Вызываем событие о загрузке
        ProfilesLoaded?.Invoke();

        // Обновляем UI/контроллер
        ProfilesListChanged?.Invoke(AllProfiles);
    }

    public UserProfile CreateProfile(string playerName, string avatarId = null)
    {
        var profile = new UserProfile(playerName, avatarId);
        _profiles.Add(profile);

        // Если это первый профиль, делаем его активным
        if (ActiveProfile == null)
            SetActiveProfile(profile.ProfileId);

        ProfilesListChanged?.Invoke(AllProfiles);
        return profile;
    }

    public void DeleteProfile(string profileId)
    {
        var profile = _profiles.FirstOrDefault(p => p.ProfileId == profileId);
        if (profile == null) return;

        _profiles.Remove(profile);

        // Если удалили активный профиль, активный становится null
        if (ActiveProfile == profile)
        {
            ActiveProfile = _profiles.FirstOrDefault();
            ActiveProfileChanged?.Invoke(ActiveProfile);
        }

        ProfilesListChanged?.Invoke(AllProfiles);
    }

    public void SetActiveProfile(string profileId)
    {
        var profile = _profiles.FirstOrDefault(p => p.ProfileId == profileId);
        if (profile == null) return;

        ActiveProfile = profile;
        ActiveProfileChanged?.Invoke(ActiveProfile);
    }

    public void SaveProfiles()
    {
        // TODO: сериализация и запись в PlayerPrefs / JSON / файл
        Debug.Log("Profiles saved. Count: " + _profiles.Count);
    }
}
