using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioService : IAudioService
{
    private const string MusicVolumeKey = "MusicVolume";
    private const string SfxVolumeKey = "SfxVolume";
    private const string MuteKey = "AudioMuted";

    public float MusicVolume { get; private set; } = 1f;
    public float SfxVolume { get; private set; } = 1f;
    public bool IsMuted { get; private set; }

    public event Action<float> MusicVolumeChanged;
    public event Action<float> SfxVolumeChanged;
    public event Action<bool> MuteStateChanged;

    private readonly AudioSource _musicSource;
    private readonly AudioSource _sfxSource;

    private readonly Dictionary<AppState, AudioClip> _musicByState;
    private readonly Dictionary<string, AudioClip> _sfxById;

    public AudioService(AudioSource musicSource, AudioSource sfxSource, Dictionary<AppState, AudioClip> musicByState, Dictionary<string, AudioClip> sfxById)
    {
        _musicSource = musicSource;
        _sfxSource = sfxSource;
        _musicByState = musicByState;
        _sfxById = sfxById;
    }

    // =====================
    // Initialization
    // =====================

    public void Initialize()
    {
        LoadSettings();
        ApplyVolumes();
    }

    public void LoadSettings()
    {
        MusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        SfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, 1f);
        IsMuted = PlayerPrefs.GetInt(MuteKey, 0) == 1;
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat(MusicVolumeKey, MusicVolume);
        PlayerPrefs.SetFloat(SfxVolumeKey, SfxVolume);
        PlayerPrefs.SetInt(MuteKey, IsMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    // =====================
    // Music
    // =====================

    public void PlayMusicForState(AppState state)
    {
        if (!_musicByState.TryGetValue(state, out var clip))
            return;

        if (_musicSource.clip == clip && _musicSource.isPlaying)
            return;

        _musicSource.clip = clip;
        _musicSource.loop = true;
        _musicSource.Play();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
        _musicSource.clip = null;
    }

    public void SetMusicVolume(float volume)
    {
        MusicVolume = Mathf.Clamp01(volume);
        ApplyVolumes();
        SaveSettings();
        MusicVolumeChanged?.Invoke(MusicVolume);
    }

    // =====================
    // SFX
    // =====================

    public void PlaySfx(string sfxId)
    {
        if (IsMuted)
            return;

        if (!_sfxById.TryGetValue(sfxId, out var clip))
            return;

        _sfxSource.PlayOneShot(clip, SfxVolume);
    }

    public void SetSfxVolume(float volume)
    {
        SfxVolume = Mathf.Clamp01(volume);
        ApplyVolumes();
        SaveSettings();
        SfxVolumeChanged?.Invoke(SfxVolume);
    }

    // =====================
    // Global
    // =====================

    public void Mute(bool mute)
    {
        IsMuted = mute;
        ApplyVolumes();
        SaveSettings();
        MuteStateChanged?.Invoke(IsMuted);
    }

    // =====================
    // Helpers
    // =====================

    private void ApplyVolumes()
    {
        float music = IsMuted ? 0f : MusicVolume;
        float sfx = IsMuted ? 0f : SfxVolume;

        _musicSource.volume = music;
        _sfxSource.volume = sfx;
    }
}
