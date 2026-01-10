using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanderHUD : MonoBehaviour
{
    [SerializeField] private Lander Lander;
    [SerializeField] private LanderFuelTank LanderFuelTank;
    [SerializeField] private LanderMover LanderMover;
    [SerializeField] private TextMeshProUGUI TextScore;
    [SerializeField] private TextMeshProUGUI TextTime;
    [SerializeField] private GameObject UpArrow;
    [SerializeField] private GameObject DownArrow;
    [SerializeField] private GameObject LeftArrow;
    [SerializeField] private GameObject RightArrow;
    [SerializeField] private Image FuelBar;

    private float _time;
    private Rigidbody2D _landerRigidbody2D;

    private void Awake()
    {
        _landerRigidbody2D = LanderMover.GetComponent<Rigidbody2D>();
        UpArrow.SetActive(false);
        DownArrow.SetActive(false);
        LeftArrow.SetActive(false);
        RightArrow.SetActive(false);
    }

    private void OnEnable()
    {
        Lander.ScoreChanged += UpdateScore;
        LanderFuelTank.FuelChanged += UpdateFuel;
    }

    private void OnDisable()
    {
        Lander.ScoreChanged -= UpdateScore;
        LanderFuelTank.FuelChanged -= UpdateFuel;
    }

    private void Start()
    {
        UpdateScore(Lander, EventArgs.Empty);
        UpdateFuel(LanderFuelTank, EventArgs.Empty);
    }

    private void Update()
    {
        UpdateTime();
        UpdateMovementArrows();
    }

    private void UpdateTime()
    {
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
        TextScore.text = Lander.Score.ToString();
    }

    private void UpdateFuel(object sender, EventArgs e)
    {
        FuelBar.fillAmount = LanderFuelTank.Fuel;
    }
}
