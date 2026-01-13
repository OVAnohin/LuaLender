using System;
using UnityEngine;
using UnityEngine.Events;

public partial class Lander : MonoBehaviour
{
    [SerializeField] private float Force = 700f;
    [SerializeField] private float TurnSpeed = 100f;
    [SerializeField] private float SoftLandingVelocity = 3f;
    [SerializeField] private float MinDotVector = .90f;
    [SerializeField] private float FuelConsumptionAmount = 1f;
    [SerializeField] private GameFlowController GameFlow;

    public float GetForce => Force;
    public float GetTurnSpeed => TurnSpeed;
    public float GetSoftLandingVelocity => SoftLandingVelocity;
    public float GetMinDotVector => MinDotVector;

    public event EventHandler ScoreChanged;
    //public event EventHandler Crashed;
    //public event EventHandler OnLanded;
    public event EventHandler<LandingScoreCalculatedEventArgs> OnLanded;

    private int _score = 0;

    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            if (value > 0)
            {
                _score += value;
                ScoreChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }


    private bool _isAlive = true;
    public bool IsAlive => _isAlive;

    private LanderFuelTank _fuelTank;
    private LanderMover _landerMover;

    private void Awake()
    {
        _fuelTank = GetComponent<LanderFuelTank>();
        _landerMover = GetComponent<LanderMover>();
        GameFlow.SetState(GameState.Ready);
    }

    private void Start()
    {
        ScoreChanged?.Invoke(this, EventArgs.Empty);
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
        LandingScoreCalculatedEventArgs eventArgs = new LandingScoreCalculatedEventArgs(landingScore, landingAngle, landingSpeed, landingType);
        OnLanded?.Invoke(this, eventArgs);
        gameObject.SetActive(false);
        GameFlow.SetState(GameState.Crashed);
    }

    private void Land(int landingScore, float landingAngle, float landingSpeed)
    {
        LandingScoreCalculatedEventArgs eventArgs = new LandingScoreCalculatedEventArgs(landingScore, landingAngle, landingSpeed, LandingType.Success);
        OnLanded?.Invoke(this, eventArgs);
        GameFlow.SetState(GameState.Landed);
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
