using UnityEngine;

[CreateAssetMenu(menuName = "Lander/Lander Config")]
public class LanderConfig : ScriptableObject
{
    public float Force = 700f;
    public float TurnSpeed = 100f;
    public float SoftLandingVelocity = 3f;
}
