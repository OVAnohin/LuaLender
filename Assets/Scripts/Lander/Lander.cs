using System;
using UnityEngine;

public class Lander : MonoBehaviour
{
    [SerializeField] private float Force = 700f;
    [SerializeField] private float TurnSpeed = 100f;
    [SerializeField] private float SoftLandingVelocity = 3f;
    [SerializeField] private float MinDotVector = .90f;
    [SerializeField] private float FuelConsumptionAmount = 1f;

    public float GetForce => Force;
    public float GetTurnSpeed => TurnSpeed;
    public float GetSoftLandingVelocity => SoftLandingVelocity;
    public float GetMinDotVector => MinDotVector;

    //public event EventHandler Crashed;
    //public event EventHandler Landed;

    private bool _isAlive = true;
    public bool IsAlive => _isAlive;

    private FuelTank _fuelTank;
    private LanderMover _landerMover;

    private void Awake()
    {
        _fuelTank = GetComponent<FuelTank>();
        _landerMover = GetComponent<LanderMover>();
    }

    private void OnEnable()
    {
        _fuelTank.FuelTankEmpty += FuelTankEmpty;
        _landerMover.OnUpForce += LanderMoverOnAllEngineForce;
        _landerMover.OnRightForce += LanderMoverOnOneEngineForce;
        _landerMover.OnLeftForce += LanderMoverOnOneEngineForce;
    }

    private void OnDisable()
    {
        _fuelTank.FuelTankEmpty -= FuelTankEmpty;
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

    private void FuelTankEmpty(object sender, EventArgs e)
    {
        Debug.Log("Fuel Tank is Empty !!!");
        return;
    }

    internal void OnLandingPadContact(LandingPad landingPad, Collision2D collision2D)
    {
        if (_isAlive == false)
            return;

        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;
        if (relativeVelocityMagnitude > SoftLandingVelocity)
        {
            Crash("Landed too hard!");
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        if (dotVector < MinDotVector)
        {
            Crash("Landed on a too steep angle!");
            return;
        }

        LandingPointsCalculation(landingPad, relativeVelocityMagnitude, dotVector);
    }

    private void LandingPointsCalculation(LandingPad landingPad, float relativeVelocityMagnitude, float dotVector)
    {
        float maxScoreAmountLandingAngle = 100f;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreAmountLandingAngle;

        float maxScoreAmountLandingSpeed = 100f;
        float landingSpeedScore = (SoftLandingVelocity - relativeVelocityMagnitude) * maxScoreAmountLandingSpeed;

        int score = Mathf.RoundToInt((landingAngleScore + landingSpeedScore) * landingPad.GetScore);
        Land(score.ToString());
    }

    internal void OnPlanetSurfaceContact()
    {
        Crash("Crashed!");
        return;
    }

    private void Crash(string message)
    {
        _isAlive = false;
        Debug.Log("BIG BAGA BOOM");
        //Crashed?.Invoke(this, EventArgs.Empty);
        Debug.Log(message);
        //Crashed?.Invoke();
        gameObject.SetActive(false);
    }

    private void Land(string message)
    {
        Debug.Log("Successful landing");
        Debug.Log(message);
        //Landed?.Invoke();
    }
}
