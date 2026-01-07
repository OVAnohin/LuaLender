using System;
using UnityEngine;

public class Lander : MonoBehaviour
{
    [SerializeField] private float Force = 700f;
    [SerializeField] private float TurnSpeed = 100f;
    [SerializeField] private float SoftLandingVelocity = 3f;
    [SerializeField] private float MinDotVector = .90f;

    public float GetForce => Force;
    public float GetTurnSpeed => TurnSpeed;
    public float GetSoftLandingVelocity => SoftLandingVelocity;
    public float GetMinDotVector => MinDotVector;

    //public event EventHandler Crashed;
    //public event EventHandler Landed;

    internal void OnLandingPadContact(LandingPad landingPad, Collision2D collision2D)
    {
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

        Debug.Log("Successful landing");

        float maxScoreAmountLandingAngle = 100f;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreAmountLandingAngle;

        float maxScoreAmountLandingSpeed = 100f;
        float landingSpeedScore = (SoftLandingVelocity - relativeVelocityMagnitude) * maxScoreAmountLandingSpeed;

        int score = Mathf.RoundToInt((landingAngleScore + landingSpeedScore) * landingPad.GetScore);
        Debug.Log(score);
    }

    internal void OnPlanetSurfaceContact()
    {
        Crash("Crashed!");
        return;
    }

    private void Crash(string message)
    {
        Debug.Log(message);
        //Crashed?.Invoke();
    }

    private void Land()
    {
        //Landed?.Invoke();
    }
}
