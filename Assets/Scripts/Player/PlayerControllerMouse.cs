using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerMouse : PlayerController
{
    [SerializeField]
    private Vector2 _moveDirection;
    [SerializeField]
    private Vector2 _lastMovementInput;

    [SerializeField]
    private bool _movementInput;

    [Header("Movement")]
    [SerializeField]
    private bool _moving;
    [SerializeField]
    private float _moveSpeed = 12.0f;

    [Header("Dashing")]
    [SerializeField]
    private bool _dashing;

    [SerializeField]
    private Vector2 _startDashingPosition;

    [SerializeField]
    private float _dashingDistance = 6, _dashingSpeed = 35, _dashCooldown = 2, _dashCooldownTimer;

    public override bool IsMoving => _moving;
    public override bool IsDashing => _dashing;

    // Define the events
    [SerializeField]
    private GeneralEvent _playerMoving, _playerStoppedMoving;

    private Vector3 _previousPosition;
    private float _previousMagnitude;

    // Define the radius within which input is ignored
    [Header("Input")]
    [SerializeField]
    private float _inputIgnoreRadius = 2.0f;

    // Store the last mouse input position
    private Vector2 _lastMousePosition;

    private void Start()
    {
        _previousPosition = transform.parent.position;
    }

    private void Update()
    {
        Move();
        Dash();
        CheckMovementStatus();
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

        if (_movementInput && !_moving) // If we have movement input and we are not moving, start moving
        {
            _moving = true;
        }
        else if (!_movementInput && _moving) // If we have no movement input and we are moving, stop moving
        {
            _moving = false;
        }

        if (_moving)
        {
            // Update the move direction using the stored mouse position
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(_lastMousePosition);
            float distance = Vector2.Distance(worldPosition, transform.parent.position);

            if (distance > _inputIgnoreRadius)
            {
                _moveDirection = (worldPosition - (Vector2)transform.parent.position).normalized;
            }
            else
            {
                _moveDirection = Vector2.zero;
            }

            Vector2 move = _moveSpeed * Time.deltaTime * _moveDirection;
            transform.parent.position += (Vector3)move;

            // Rotate the parent to face the movement direction
            if (_moveDirection != Vector2.zero)
            {
                float angle = Mathf.Atan2(_moveDirection.y, _moveDirection.x) * Mathf.Rad2Deg;
                transform.parent.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }
    }

    public void StartDashing()
    {
        if (_dashCooldownTimer > 0 || _moveDirection == Vector2.zero) return;

        _startDashingPosition = transform.parent.position;
        _dashing = true;
        _dashCooldownTimer = _dashCooldown;
    }

    public void OnMouseInput(InputAction.CallbackContext context)
    {
        // Store the last mouse input position
        _lastMousePosition = context.ReadValue<Vector2>();
    }

    public void OnMovementInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _movementInput = true;
        }
        else if (context.canceled)
        {
            _movementInput = false;
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

    private void OnDrawGizmos()
    {
        // Set the color for the Gizmos
        Gizmos.color = Color.red;

        // Draw a wireframe sphere at the player's position with the radius of _inputIgnoreRadius
        Gizmos.DrawWireSphere(transform.parent.position, _inputIgnoreRadius);
    }
}
