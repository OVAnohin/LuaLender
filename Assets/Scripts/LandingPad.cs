using UnityEngine;

public class LandingPad : MonoBehaviour
{
    [SerializeField] private int ScoreMultipler;

    public int GetScore => ScoreMultipler;
}
