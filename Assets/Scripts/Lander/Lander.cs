using System;
using UnityEngine;

public partial class Lander : MonoBehaviour
{
    [SerializeField] private LanderConfig Config;
    [SerializeField] private float MinDotVector = .90f;
    [SerializeField] private float FuelConsumptionAmount = 1f;
    [SerializeField] private LevelStateController GameFlow;
    [SerializeField] private SpriteRenderer SpriteRenderer;

    public event EventHandler ScoreChanged;
    public event EventHandler Crashed;
    public event EventHandler<LanderScoreCalculatedEventArgs> Landed;

    private bool _isInitialized = false;

    private int _score = 0;
    public int Score => _score;

    private bool _isAlive = true;
    public bool IsAlive => _isAlive;
    public float SoftLandingVelocity => Config.SoftLandingVelocity;

    private LanderFuelTank _fuelTank;
    private LanderMover _landerMover;

    public void Initialize(LevelStateController controller, int score)
    {
        if (_isInitialized)
            return;

        GameFlow = controller;
        _score = score;
        _isInitialized = true;

        _landerMover.Initialize(GameFlow);
    }

    private void Awake()
    {
        _fuelTank = GetComponent<LanderFuelTank>();
        _landerMover = GetComponent<LanderMover>();
    }

    private void OnEnable()
    {
        _landerMover.OnUpForce += LanderMoverOnAllEngineForce;
        _landerMover.OnRightForce += LanderMoverOnOneEngineForce;
        _landerMover.OnLeftForce += LanderMoverOnOneEngineForce;
    }

    private void OnDisable()
    {
        _landerMover.OnUpForce -= LanderMoverOnAllEngineForce;
        _landerMover.OnRightForce -= LanderMoverOnOneEngineForce;
        _landerMover.OnLeftForce -= LanderMoverOnOneEngineForce;
    }

    private void LanderMoverOnOneEngineForce(object sender, EventArgs e)
    {
        int engineCount = 1;
        _fuelTank.Consume(engineCount, FuelConsumptionAmount);
    }

    private void LanderMoverOnAllEngineForce(object sender, EventArgs e)
    {
        int engineCount = 3;
        _fuelTank.Consume(engineCount, FuelConsumptionAmount);
    }

    internal void OnLandingPadContact(LandingPad landingPad, Collision2D collision2D)
    {
        if (_isAlive == false)
            return;

        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;
        if (relativeVelocityMagnitude > SoftLandingVelocity)
        {
            Crash(0, 0, relativeVelocityMagnitude, LandingType.TooFastLanding);
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        if (dotVector < MinDotVector)
        {
            Crash(0, dotVector, relativeVelocityMagnitude, LandingType.TooStepAngle);
            return;
        }

        int landingScore = LandingPointsCalculation(landingPad, relativeVelocityMagnitude, dotVector);
        _score += landingScore;
        ScoreChanged?.Invoke(this, EventArgs.Empty);
        Land(landingScore, dotVector, relativeVelocityMagnitude);
    }

    private int LandingPointsCalculation(LandingPad landingPad, float relativeVelocityMagnitude, float dotVector)
    {
        float maxScoreAmountLandingAngle = 100f;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreAmountLandingAngle;

        float maxScoreAmountLandingSpeed = 100f;
        float landingSpeedScore = (SoftLandingVelocity - relativeVelocityMagnitude) * maxScoreAmountLandingSpeed;

        return Mathf.RoundToInt((landingAngleScore + landingSpeedScore) * landingPad.GetScore);
    }

    internal void OnPlanetSurfaceContact()
    {
        int landingScore = 0;
        float landingAngle = 0;
        float landingSpeed = 0;
        Crash(landingScore, landingAngle, landingSpeed, LandingType.WrongLandingArea);
    }

    private void Crash(int landingScore, float landingAngle, float landingSpeed, LandingType landingType)
    {
        _isAlive = false;
        GameFlow.SetState(LevelPhase.Crashed);
        LanderScoreCalculatedEventArgs eventArgs = new LanderScoreCalculatedEventArgs(landingScore, landingAngle, landingSpeed, landingType);
        SpriteRenderer.enabled = false;
        Landed?.Invoke(this, eventArgs);
        Crashed?.Invoke(this, EventArgs.Empty);
        gameObject.SetActive(false);
    }

    private void Land(int landingScore, float landingAngle, float landingSpeed)
    {
        LanderScoreCalculatedEventArgs eventArgs = new LanderScoreCalculatedEventArgs(landingScore, landingAngle, landingSpeed, LandingType.Success);
        Landed?.Invoke(this, eventArgs);
        GameFlow.SetState(LevelPhase.Landed);
    }

    internal void OnFuelPickupContact(FuelPickup fuelPickup)
    {
        _fuelTank.AddFuel = fuelPickup.GetVolume;
    }

    internal void OnCoinPickupContact(CoinPickup coinPickup)
    {
        _score += coinPickup.GetPoints;
        ScoreChanged?.Invoke(this, EventArgs.Empty);
    }
}
