using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanderHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TextScore;
    [SerializeField] private TextMeshProUGUI TextTime;
    [SerializeField] private GameObject UpArrow;
    [SerializeField] private GameObject DownArrow;
    [SerializeField] private GameObject LeftArrow;
    [SerializeField] private GameObject RightArrow;
    [SerializeField] private Image FuelBar;
    [SerializeField] private GameFlowController GameFlow;
    [SerializeField] private LevelInitializer LevelInitializer;

    private float _time;
    private Lander _lander;
    private LanderFuelTank _landerFuelTank;
    private Rigidbody2D _landerRigidbody2D;

    private void Awake()
    {
        UpArrow.SetActive(false);
        DownArrow.SetActive(false);
        LeftArrow.SetActive(false);
        RightArrow.SetActive(false);
    }

    private void OnEnable()
    {
        LevelInitializer.LanderSpawned += OnLanderSpawned;
        LevelInitializer.LanderDestroyed += OnLanderDestroyed;
    }

    private void OnLanderSpawned(object sender, LanderArgs lander)
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

    private void OnDisable()
    {
        LevelInitializer.LanderSpawned -= OnLanderSpawned;
        LevelInitializer.LanderDestroyed -= OnLanderDestroyed;
    }

    private void OnLanderDestroyed(object sender, LanderArgs lander)
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
        if (GameFlow.CurrentState != GamePhase.Playing)
            return;

        _time += Time.deltaTime;
        TextTime.text = _time.ToString();
    }

    private void UpdateMovementArrows()
    {
        Vector2 v = _landerRigidbody2D.linearVelocity;
        const float threshold = 0.1f;

        UpArrow.SetActive(v.y > threshold);
        DownArrow.SetActive(v.y < -threshold);
        RightArrow.SetActive(v.x > threshold);
        LeftArrow.SetActive(v.x < -threshold);
    }

    private void UpdateScore(object sender, EventArgs e)
    {
        SaveService.Save(new SaveData(_lander.Score));
        TextScore.text = _lander.Score.ToString();
    }

    private void UpdateFuel(object sender, EventArgs e)
    {
        FuelBar.fillAmount = _landerFuelTank.Fuel;
    }
}
