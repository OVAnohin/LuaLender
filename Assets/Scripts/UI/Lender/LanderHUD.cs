using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanderHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textScore;
    [SerializeField] private TextMeshProUGUI textTime;
    [SerializeField] private GameObject upArrow;
    [SerializeField] private GameObject downArrow;
    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;
    [SerializeField] private Image fuelBar;
    [SerializeField] private LevelStateController levelStateController;
    [SerializeField] private LevelInitializer levelInitializer;

    private float _time;
    private Lander _lander;
    private LanderFuelTank _landerFuelTank;
    private Rigidbody2D _landerRigidbody2D;

    private void Awake()
    {
        upArrow.SetActive(false);
        downArrow.SetActive(false);
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
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

        LanderMover landerMover = _lander.GetComponent<LanderMover>();
        _landerRigidbody2D = landerMover.GetComponent<Rigidbody2D>();

        _landerFuelTank = _lander.GetComponent<LanderFuelTank>();

        _lander.ScoreChanged += UpdateScore;
        _landerFuelTank.FuelChanged += UpdateFuel;
        UpdateScore(null, EventArgs.Empty);
    }

    private void OnLanderDestroyed(object sender, LanderEventArgs lander)
    {
        if (lander.Lander == _lander)
            Unsubscribe();
    }

    private void Update()
    {
        if (_lander == null)
            return;

        UpdateTime();
        UpdateMovementArrows();
    }

    private void Unsubscribe()
    {
        if (_lander != null)
            _lander.ScoreChanged -= UpdateScore;

        if (_landerFuelTank != null)
            _landerFuelTank.FuelChanged -= UpdateFuel;

        _lander = null;
        _landerFuelTank = null;
    }

    private void UpdateTime()
    {
        if (levelStateController.CurrentPhase != LevelPhase.Playing)
            return;

        _time += Time.deltaTime;
        textTime.text = _time.ToString();
    }

    private void UpdateMovementArrows()
    {
        Vector2 v = _landerRigidbody2D.linearVelocity;
        const float threshold = 0.1f;

        upArrow.SetActive(v.y > threshold);
        downArrow.SetActive(v.y < -threshold);
        rightArrow.SetActive(v.x > threshold);
        leftArrow.SetActive(v.x < -threshold);
    }

    private void UpdateScore(object sender, EventArgs e)
    {
        textScore.text = _lander.Score.ToString();
    }

    private void UpdateFuel(object sender, EventArgs e)
    {
        fuelBar.fillAmount = _landerFuelTank.Fuel;
    }
}
