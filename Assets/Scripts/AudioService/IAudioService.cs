using System;
using UnityEngine;

public interface IAudioService
{
    // ======================
    // Состояние
    // ======================

    float MusicVolume { get; }
    float SfxVolume { get; }
    bool IsMuted { get; }

    // ======================
    // События
    // ======================

    event Action<float> MusicVolumeChanged;
    event Action<float> SfxVolumeChanged;
    event Action<bool> MuteStateChanged;

    // ======================
    // Инициализация
    // ======================

    void Initialize();
    void LoadSettings();
    void SaveSettings();

    // ======================
    // Музыка
    // ======================

    void PlayMusicForState(AppState state);
    void StopMusic();

    void SetMusicVolume(float volume);

    // ======================
    // Звуковые эффекты
    // ======================

    void PlaySfx(string sfxId);
    void SetSfxVolume(float volume);

    // ======================
    // Глобальные операции
    // ======================

    void Mute(bool mute);

    // =====================
    // Engine
    // =====================

    public void PlayEngineLoop();
    public void StopEngineLoop();
}
