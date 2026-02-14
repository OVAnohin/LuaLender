using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AppFlowController))]
public class AppBootstrap : MonoBehaviour
{
    [SerializeField] private AudioSource MusicSource;
    [SerializeField] private AudioSource SfxSource;

    [SerializeField] private AudioClip GameplayMusic;
    [SerializeField] private AudioClip MenuMusic;

    [SerializeField] private AudioClip CoinSfx;
    [SerializeField] private AudioClip CrashSfx;
    [SerializeField] private AudioClip FuelPickupSfx;
    [SerializeField] private AudioClip LandingSuccessSfx;
    [SerializeField] private AudioClip EngineSfx;

    public static AppBootstrap Instance { get; private set; }

    public AppFlowController AppFlow { get; private set; }
    public ProfileService ProfileService { get; private set; }

    public IAudioService AudioService { get; private set; }

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Инициализация сервисов
        AppFlow = GetComponent<AppFlowController>();
        if (AppFlow == null)
            Debug.LogError("AppFlowController не найден на AppBootstrap!");

        ProfileService = new ProfileService();
        ProfileService.LoadProfiles();

        var musicMap = new Dictionary<AppState, AudioClip>
        {
            { AppState.MainMenu, MenuMusic },
            { AppState.Gameplay, GameplayMusic }
        };

        var sfxMap = new Dictionary<string, AudioClip>
        {
            { "Coin", CoinSfx },
            { "Crash", CrashSfx },
            { "FuelPickup", FuelPickupSfx },
            { "LandingSuccess", LandingSuccessSfx },
            { "Engine", EngineSfx }
        };

        AudioService = new AudioService(
            MusicSource,
            SfxSource,
            musicMap,
            sfxMap
        );

        AudioService.Initialize();

        AppFlow.StateChanged += OnAppStateChanged;
    }

    private void OnAppStateChanged(AppState state)
    {
        AudioService.PlayMusicForState(state);
    }

    private void Start()
    {
        AppFlow?.SetState(AppState.MainMenu);
    }
}
