using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] private LevelInitializer levelInitializer;
    [SerializeField] private LandedUI landedUI;

    private Lander _lander;
    private ProfileService _profileService;
    private AppFlowController _appFlow;

    private void Awake()
    {
        _profileService = AppBootstrap.Instance.ProfileService;
        _appFlow = AppBootstrap.Instance.AppFlow;

        landedUI.Initialize(this);
    }

    private void OnEnable()
    {
        levelInitializer.LanderSpawned += OnLanderSpawned;
        levelInitializer.LanderDestroyed += OnLanderDestroyed;
    }

    private void OnDisable()
    {
        levelInitializer.LanderSpawned -= OnLanderSpawned;
        levelInitializer.LanderDestroyed -= OnLanderDestroyed;
    }

    private void OnLanderSpawned(object sender, LanderEventArgs lander)
    {
        Unsubscribe();

        _lander = lander.Lander;
        _lander.Landed += LanderOnLanded;
    }

    private void OnLanderDestroyed(object sender, LanderEventArgs lander)
    {
        if (lander.Lander == _lander)
            Unsubscribe();
    }

    private void Unsubscribe()
    {
        if (_lander != null)
            _lander.Landed -= LanderOnLanded;

        _lander = null;
    }

    private void LanderOnLanded(object sender, Lander.LanderScoreCalculatedEventArgs args)
    {
        UserProfile userProfile = _profileService.ActiveProfile;

        if (args.LandingType.Equals(Lander.LandingType.Success))
            userProfile.Statistics.RegisterWin(args.Score, 0);
        else
            userProfile.Statistics.RegisterLose(args.Score, 0);

        _profileService.SaveProfiles();
    }

    internal void OnRestartClicked()
    {
        SceneManager.LoadScene(SceneNames.GameScene.ToString());
    }

    internal void OnReturnMainMenuClicked()
    {
        _appFlow.SetState(AppState.MainMenu);
    }
}
