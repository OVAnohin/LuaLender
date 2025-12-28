using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Lander : MonoBehaviour
{
    [SerializeField] private float Force = 700f;
    [SerializeField] private float TurnSpeed = 100f;
    [SerializeField] private float SoftLandingVelocity = 3f;
    [SerializeField] private float MinDotVector = .90f;

    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Keyboard.current.upArrowKey.isPressed)
        {
            _rigidbody2D.AddForce(Force * transform.up * Time.deltaTime);
        }

        if (Keyboard.current.leftArrowKey.isPressed)
        {
            _rigidbody2D.AddTorque(TurnSpeed * Time.deltaTime);
        }

        if (Keyboard.current.rightArrowKey.isPressed)
        {
            _rigidbody2D.AddTorque(-TurnSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (!collision2D.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            Debug.Log("Crashed!");
            return;
        }

        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;
        if (relativeVelocityMagnitude > SoftLandingVelocity)
        {
            Debug.Log("Landed too hard!");
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        if (dotVector < MinDotVector)
        {
            Debug.Log("Landed on a too steep angle!");
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
}
