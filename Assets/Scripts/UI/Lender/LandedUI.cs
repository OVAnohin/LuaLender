using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleTextMesh;
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private LevelInitializer levelInitializer;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button returnMainMenuButton;

    private Lander _lander;
    private CanvasGroup _canvasGroup;
    //private LevelController _levelController;

    public void Initialize(LevelController levelController)
    {
        restartButton.onClick.AddListener(levelController.OnRestartClicked);
        returnMainMenuButton.onClick.AddListener(levelController.OnReturnMainMenuClicked);

        //Subscribe();
    }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>(); 
        //nextButton.onClick.AddListener(OnNextClicked);
        Hide();
    }

    //private void OnNextClicked()
    //{
    //    SceneManager.LoadScene(SceneNames.GameScene.ToString());
    //}

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

    //private void Subscribe()
    //{
    //    _mainMenuController.UpdatePlayButtonStatus += UpdatePlayButtonState;
    //    _mainMenuController.UpdateUserName += UpdateUserName;
    //}

    //public void Deinitialize()
    //{
    //    if (_mainMenuController == null)
    //        return;

    //    _mainMenuController.UpdatePlayButtonStatus -= UpdatePlayButtonState;
    //    _mainMenuController.UpdateUserName -= UpdateUserName;
    //}

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
        if (args.LandingType.Equals(Lander.LandingType.Success))
        {
            titleTextMesh.text = "Successful Landing!";
        }
        else
        {
            titleTextMesh.color = Color.red;
            titleTextMesh.text = args.LandingType.ToString();
        }

        statsTextMesh.text =
            args.LandingSpeed + "\n" +
            args.LandingAngle + "\n" +
            0 + "\n" +
            args.Score + "\n";

        StartCoroutine(PauseBeforeShow(.5f));
    }

    private IEnumerator PauseBeforeShow(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Show();
    }

    private void Show()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    private void Hide()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
}
