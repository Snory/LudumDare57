using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerGamepad : PlayerController
{
    [SerializeField]
    private Vector2 _moveDirection;

    [Header("Movement")]
    [SerializeField]
    private bool _moving;
    [SerializeField]
    private float _moveSpeed = 12.0f;
    private Vector2 _lastMovementInput, _previousMovementInput;

    [SerializeField]
    private int _sameInputforFrames = 10;
    private int _frameRateCount = 0;

    [Header("Dashing")]
    [SerializeField]
    private bool _dashing;

    [SerializeField]
    private Vector2 _startDashingPosition;

    [SerializeField]
    private float _dashingDistance = 6, _dashingSpeed = 35, _dashCooldown = 2, _dashCooldownTimer;

    private bool _controller;

    public override bool IsMoving => _moving;
    public override bool IsDashing => _dashing;


    // Define the events
    [SerializeField]
    private GeneralEvent _playerMoving, _playerStoppedMoving;

    private Vector3 _previousPosition;
    private float _previousMagnitude;

    private void Start()
    {
        _previousPosition = transform.parent.position;
    }

    private void Update()
    {
        Move();
        Dash();
        CheckMovementStatus();

        _previousMovementInput = _lastMovementInput;
    }

    private void Dash()
    {
        _dashCooldownTimer -= Time.deltaTime;

        if (_dashing)
        {
            Vector2 dashDirection = _dashingSpeed * Time.deltaTime * _moveDirection;
            transform.parent.position += (Vector3)dashDirection;
        }

        if (Vector2.Distance(_startDashingPosition, transform.parent.position) > _dashingDistance)
        {
            _dashing = false;
        }
    }

    private void Move()
    {
        if (_dashing) return;

        if (!_moving && _lastMovementInput != Vector2.zero) // If we are not moving and we have input, start moving
        {
            _moving = true;
            _moveDirection = _lastMovementInput;
        }
        else if (_moving && _lastMovementInput == Vector2.zero) // If we are moving and we have no input, stop moving
        {
            _moving = false;
        }

        // Adjust movement to handle input release
        if (!_controller)
        {
            KeyboardMovementAdjustments();
        }
        else
        {
            ControllerAdjustments();
        }

        if (_moving)
        {
            Vector2 move = _moveSpeed * Time.deltaTime * _moveDirection;
            transform.parent.position += (Vector3)move;
        }
    }

    private void ControllerAdjustments()
    {
        _moveDirection = _lastMovementInput;
    }

    private void KeyboardMovementAdjustments()
    {
        if (_lastMovementInput == _previousMovementInput)
        {
            _frameRateCount++;
        }
        else
        {
            _frameRateCount = 0;
        }

        if (_frameRateCount > _sameInputforFrames && _moveDirection != _lastMovementInput)
        {
            _moveDirection = _lastMovementInput;
        }
    }

    public void StartDashing()
    {
        if (_dashCooldownTimer > 0 || _moveDirection == Vector2.zero) return;

        _startDashingPosition = transform.parent.position;
        _dashing = true;
        _dashCooldownTimer = _dashCooldown;
    }

    public void OnMovementInput(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            Vector2 inputMoveVector = context.ReadValue<Vector2>();
            _lastMovementInput = inputMoveVector;

            if (context.control.device is Gamepad)
            {
                _controller = true;
            }
            else
            {
                _controller = false;
            }
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartDashing();
        }
    }

    private void CheckMovementStatus()
    {
        Vector3 currentPosition = transform.parent.position;
        Vector3 velocity = (currentPosition - _previousPosition) / Time.deltaTime;

        // založit si boolean na to, zda mám raisovat tj. jestli se změnil magnitude
        if (velocity.magnitude > 0 && _previousMagnitude == 0)
        {
            _playerMoving.Raise();
        }
        else if (velocity.magnitude == 0 && _previousMagnitude > 0)
        {
            _playerStoppedMoving.Raise();
        }

        _previousPosition = currentPosition;
        _previousMagnitude = velocity.magnitude;
    }
}
