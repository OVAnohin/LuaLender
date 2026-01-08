using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class LanderMover : MonoBehaviour
{
    public event EventHandler OnUpForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnBeforeForce;

    private Rigidbody2D _rigidbody2D;
    private Lander _lander;
    private FuelTank _fuelTank;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _lander = GetComponent<Lander>();
        _fuelTank = GetComponent<FuelTank>();
    }

    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        if (!_fuelTank.HasFuel)
            return;

        if (Keyboard.current.upArrowKey.isPressed)
        {
            _rigidbody2D.AddForce(_lander.GetForce * transform.up * Time.deltaTime);
            OnUpForce?.Invoke(this, EventArgs.Empty);
        }

        if (Keyboard.current.leftArrowKey.isPressed)
        {
            _rigidbody2D.AddTorque(_lander.GetTurnSpeed * Time.deltaTime);
            OnLeftForce?.Invoke(this, EventArgs.Empty);
        }

        if (Keyboard.current.rightArrowKey.isPressed)
        {
            _rigidbody2D.AddTorque(-_lander.GetTurnSpeed * Time.deltaTime);
            OnRightForce?.Invoke(this, EventArgs.Empty);
        }
    }
}
