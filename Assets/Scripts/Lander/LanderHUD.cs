using System;
using TMPro;
using UnityEngine;

public class LanderHUD : MonoBehaviour
{
    [SerializeField] private Lander Lander;
    [SerializeField] private LanderFuelTank LanderFuelTank;
    [SerializeField] private LanderMover LanderMover;
    [SerializeField] private TextMeshProUGUI TextScore;
    [SerializeField] private TextMeshProUGUI TextTime;
    [SerializeField] private TextMeshProUGUI TextFuel;
    [SerializeField] private GameObject ImageSpeedUp;
    [SerializeField] private GameObject ImageSpeedDown;
    [SerializeField] private GameObject ImageSpeedLeft;
    [SerializeField] private GameObject ImageSpeedRight;

    private float _time;
    private Rigidbody2D _landerRigidbody2D;

    private void Awake()
    {
        _landerRigidbody2D = LanderMover.GetComponent<Rigidbody2D>();
        ImageSpeedUp.SetActive(false);
        ImageSpeedDown.SetActive(false);
        ImageSpeedLeft.SetActive(false);
        ImageSpeedRight.SetActive(false);
    }

    private void OnEnable()
    {
        Lander.ScoreChanged += UpdateScore;
        LanderFuelTank.FuelChanged += UpdateFuel;
        LanderMover.OnUpForce += LanderUpForce;
        LanderMover.OnLeftForce += LanderLeftForce;
        LanderMover.OnRightForce += LanderRightForce;
    }

    private void OnDisable()
    {
        Lander.ScoreChanged -= UpdateScore;
        LanderFuelTank.FuelChanged -= UpdateFuel;
        LanderMover.OnUpForce -= LanderUpForce;
        LanderMover.OnLeftForce -= LanderLeftForce;
        LanderMover.OnRightForce -= LanderRightForce;
    }

    private void Start()
    {
        UpdateScore(Lander, EventArgs.Empty);
        UpdateFuel(LanderFuelTank, EventArgs.Empty);
    }

    private void Update()
    {
        UpdateTime();
        LanderDownForce();
    }

    private void UpdateTime()
    {
        _time += Time.deltaTime;
        TextTime.text = _time.ToString();
    }

    private void LanderUpForce(object sender, EventArgs e)
    {
        ImageSpeedDown.SetActive(false);
        ImageSpeedUp.SetActive(true);
    }

    private void LanderDownForce()
    {
        if (_landerRigidbody2D.linearVelocityY < 0)
        {
            ImageSpeedUp.SetActive(false);
            ImageSpeedDown.SetActive(true);
        }
    }

    private void LanderRightForce(object sender, EventArgs e)
    {
        ImageSpeedRight.SetActive(false);
        ImageSpeedLeft.SetActive(true);
    }

    private void LanderLeftForce(object sender, EventArgs e)
    {
        ImageSpeedLeft.SetActive(false);
        ImageSpeedRight.SetActive(true);
    }

    private void UpdateScore(object sender, EventArgs e)
    {
        TextScore.text = Lander.Score.ToString();
    }

    private void UpdateFuel(object sender, EventArgs e)
    {
        TextFuel.text = LanderFuelTank.Fuel.ToString();
    }
}
