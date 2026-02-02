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

        _lander.ScoreChanged += UpdateScore;
        _lander.Landed += LanderOnLanded;
        UpdateScore(null, EventArgs.Empty);
    }

    private void OnLanderDestroyed(object sender, LanderEventArgs lander)
    {
        if (lander.Lander == _lander)
            Unsubscribe();
    }

    private void Unsubscribe()
    {
        if (_lander != null)
        {
            _lander.ScoreChanged -= UpdateScore;
            _lander.Landed -= LanderOnLanded;
        }
            
        _lander = null;
    }

    private void UpdateScore(object sender, EventArgs e)
    {
        Debug.Log("UpdateScore");
    }

    private void LanderOnLanded(object sender, Lander.LanderScoreCalculatedEventArgs args)
    {
        //if (args.LandingType.Equals(Lander.LandingType.Success))
        //{
        //    titleTextMesh.text = "Successful Landing!";
        //}
        //else
        //{
        //    titleTextMesh.color = Color.red;
        //    titleTextMesh.text = args.LandingType.ToString();
        //}
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
