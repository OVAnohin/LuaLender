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

    private InputAction _anyKeyAction;

    private Rigidbody2D _rigidbody2D;
    private LanderFuelTank _fuelTank;
    private GameFlowController _gameFlowController;
    private bool _isInitialized = false;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _fuelTank = GetComponent<LanderFuelTank>();
        _rigidbody2D.gravityScale = 0f;

        _anyKeyAction = new InputAction(
            name: "AnyKey",
            type: InputActionType.Button,
            binding: "<Keyboard>/anyKey"
        );
    }

    public void Initialize(GameFlowController gameFlowController)
    {
        if (_isInitialized)
            return;

        _gameFlowController = gameFlowController;
        _isInitialized = true;
    }

    private void OnEnable()
    {
        _anyKeyAction.Enable();
        _anyKeyAction.performed += OnAnyKeyPressed;
    }

    private void OnDisable()
    {
        _anyKeyAction.performed -= OnAnyKeyPressed;
        _anyKeyAction.Disable();
    }

    private void OnAnyKeyPressed(InputAction.CallbackContext ctx)
    {
        if (_gameFlowController.CurrentState == GameState.Ready)
        {
            _rigidbody2D.gravityScale = GRAVITY_NORMAL;
            _gameFlowController.SetState(GameState.Playing);
        }
    }

    private void FixedUpdate()
    {
        bool engineActive = false;

        if (_gameFlowController.CurrentState != GameState.Playing || !_fuelTank.HasFuel)
        {
            EngineStateChanged?.Invoke(false);
            return;
        }

        if (Keyboard.current.upArrowKey.isPressed)
        {
            _rigidbody2D.AddForce(Force * transform.up * Time.deltaTime);
            OnUpForce?.Invoke(this, EventArgs.Empty);
            engineActive = true;
        }

        if (Keyboard.current.leftArrowKey.isPressed)
        {
            _rigidbody2D.AddTorque(TurnSpeed * Time.deltaTime);
            OnLeftForce?.Invoke(this, EventArgs.Empty);
            engineActive = true;
        }

        if (Keyboard.current.rightArrowKey.isPressed)
        {
            _rigidbody2D.AddTorque(-TurnSpeed * Time.deltaTime);
            OnRightForce?.Invoke(this, EventArgs.Empty);
            engineActive = true;
        }

        EngineStateChanged?.Invoke(engineActive);
    }
}
