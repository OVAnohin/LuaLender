using UnityEngine;

public class LanderCollision : MonoBehaviour
{
    private Lander _lander;

    private void Awake()
    {
        _lander = GetComponent<Lander>();
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.collider.TryGetComponent(out LandingPad landingPad))
        {
            _lander.OnLandingPadContact(landingPad, collision2D);
        }
        else
        {
            _lander.OnPlanetSurfaceContact();
        }
    }
}
