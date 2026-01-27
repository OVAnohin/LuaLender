using UnityEngine;

public class LandingPad : MonoBehaviour
{
    [SerializeField] private int scoreMultipler;

    public int GetScore => scoreMultipler;
}
