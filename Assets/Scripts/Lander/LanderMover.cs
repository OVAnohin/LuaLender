using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class LanderMover : MonoBehaviour
{
    [SerializeField] private LanderConfig Config;

    private const float GRAVITY_NORMAL = 0.7f;

    public event EventHandler OnUpForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnLeftForce;
    public event Action<bool> EngineStateChanged;

    public float Force => Config.Force;
    public float TurnSpeed => Config.TurnSpeed;

    private Rigidbody2D _rigidbody2D;
    private LanderFuelTank _fuelTank;
    private LevelStateController _gameFlowController;
    private LanderInputActions _input;
    private bool _isInitialized = false;

    private float _thrustInput;
    private float _turnInput;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _fuelTank = GetComponent<LanderFuelTank>();
        _rigidbody2D.gravityScale = 0f;

        _input = new LanderInputActions();
    }

    public void Initialize(LevelStateController gameFlowController)
    {
        if (_isInitialized)
            return;

        _gameFlowController = gameFlowController;
        _isInitialized = true;
    }

    private void OnEnable()
    {
        _input.Gameplay.StartGame.performed += OnAnyKeyPressed;
        _input.Gameplay.Thrust.performed += OnThrust;
        _input.Gameplay.Thrust.canceled += OnThrustCanceled;

        _input.Gameplay.TurnRight.performed += OnTurnRightPerformed;
        _input.Gameplay.TurnRight.canceled += OnTurnRightCanceled;
        _input.Gameplay.TurnLeft.performed += OnTurnLeftPerformed;
        _input.Gameplay.TurnLeft.canceled += OnTurnLeftCanceled;

        _input.Gameplay.Movement.performed += OnMovementPerformed;
        _input.Gameplay.Movement.canceled += OnMovementCanceled;

        _input.Gameplay.Enable();
    }

    private void OnDisable()
    {
        _input.Gameplay.StartGame.performed -= OnAnyKeyPressed;
        _input.Gameplay.Thrust. performed -= OnThrust;
        _input.Gameplay.Thrust.canceled -= OnThrustCanceled;

        _input.Gameplay.TurnRight.performed -= OnTurnRightPerformed;
        _input.Gameplay.TurnRight.canceled -= OnTurnRightCanceled;
        _input.Gameplay.TurnLeft.performed -= OnTurnLeftPerformed;
        _input.Gameplay.TurnLeft.canceled -= OnTurnLeftCanceled;

        _input.Gameplay.Movement.performed -= OnMovementPerformed;
        _input.Gameplay.Movement.canceled -= OnMovementCanceled;

        _input.Gameplay.Disable();
    }

    private void OnAnyKeyPressed(InputAction.CallbackContext ctx)
    {
        if (_gameFlowController.CurrentState != LevelPhase.Ready)
            return;

        _rigidbody2D.gravityScale = GRAVITY_NORMAL;
        _gameFlowController.SetState(LevelPhase.Playing);
    }

    private void OnThrust(InputAction.CallbackContext ctx)
    {
        _thrustInput = ctx.ReadValue<float>();
    }

    private void OnThrustCanceled(InputAction.CallbackContext ctx)
    {
        _thrustInput = 0f;
    }

    private void OnTurnRightPerformed(InputAction.CallbackContext ctx)
    {
        _turnInput = 1f;
    }

    private void OnTurnRightCanceled(InputAction.CallbackContext ctx)
    {
        _turnInput = 0f;
    }

    private void OnTurnLeftPerformed(InputAction.CallbackContext ctx)
    {
        _turnInput = -1f;
    }

    private void OnTurnLeftCanceled(InputAction.CallbackContext ctx)
    {
        _turnInput = 0f;
    }

    private void OnMovementPerformed(InputAction.CallbackContext ctx)
    {
        Vector2 value = ctx.ReadValue<Vector2>();

        float gamepadDeadzone = .4f;
        if (value.x > gamepadDeadzone || value.x < -gamepadDeadzone)
            _turnInput = value.x;

        if (value.y > gamepadDeadzone)
            _thrustInput = value.y; 
    }

    private void OnMovementCanceled(InputAction.CallbackContext ctx)
    {
        Vector2 value = ctx.ReadValue<Vector2>();

        _turnInput = value.x;
        _thrustInput = value.y;
    }

    private void FixedUpdate()
    {
        bool engineActive = false;

        if (_gameFlowController.CurrentState != LevelPhase.Playing || !_fuelTank.HasFuel)
        {
            EngineStateChanged?.Invoke(false);
            return;
        }

        if (_thrustInput > 0f)
        {
            _rigidbody2D.AddForce(Force * transform.up * _thrustInput * Time.fixedDeltaTime);
            OnUpForce?.Invoke(this, EventArgs.Empty);
            engineActive = true;
        }

        if (_turnInput > 0f)
        {
            _rigidbody2D.AddTorque(TurnSpeed * -_turnInput * Time.fixedDeltaTime);
            OnRightForce?.Invoke(this, EventArgs.Empty);
            engineActive = true;
        }
        else if (_turnInput < 0f)
        {
            _rigidbody2D.AddTorque(TurnSpeed * -_turnInput * Time.fixedDeltaTime);
            OnLeftForce?.Invoke(this, EventArgs.Empty);
            engineActive = true;
        }

        EngineStateChanged?.Invoke(engineActive);
    }

}
