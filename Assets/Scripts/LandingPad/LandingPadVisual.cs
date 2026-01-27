using TMPro;
using UnityEngine;

[RequireComponent(typeof(LandingPad))]
public class LandingPadVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;

    private void Awake()
    {
        LandingPad landingPad = GetComponent<LandingPad>();
        textMeshPro.text = $"x {landingPad.GetScore}";
    }
}
