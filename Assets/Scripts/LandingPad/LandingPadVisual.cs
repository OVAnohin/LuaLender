using TMPro;
using UnityEngine;

[RequireComponent(typeof(LandingPad))]
public class LandingPadVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro TextMeshPro;

    private void Awake()
    {
        LandingPad landingPad = GetComponent<LandingPad>();
        TextMeshPro.text = $"x {landingPad.GetScore}";
    }
}
