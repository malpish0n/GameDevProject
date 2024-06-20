using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to whole player object")]
    [SerializeField] private Transform _player;
    private Rigidbody _rb;
    private Animator _animator;

    [Header("Move speed values")]
    [SerializeField] private float _movementSpeed;
    public float _walkSpeed;
    public float _runSpeed;
    public float _wallRunSpeed;
    public float _dashSpeed;

    [Header("Jump")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _airMultiplayer;
    private bool _canJump = true;

    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _groundDrag;
    [SerializeField] private float sphereRadius;

    [SerializeField] private float _maxSlopeAngle;
    private RaycastHit _slopeHit;

    [HideInInspector] public float _hInput, _vInput;
    private Vector3 _moveDirection;

    [Header("State bools")]
    public bool _isGrounded;
    public bool _isRunning;
    public bool _isWalking;
    public bool _isWallrunning;
    private bool _isSliding;
    private bool _isDashing;
    [SerializeField] private bool _isOnWall;
    public bool _isFreezed;
    public bool _isUnlimited;
    public bool _isRestricted;

    public bool IsSliding
    {
        get { return _isSliding; }
        set { _isSliding = value; }
    }

    public bool IsDashing
    {
        get { return _isDashing; }
        set { _isDashing = value; }
    }

    public bool IsOnWall
    {
        get { return _isOnWall; }
        set { _isOnWall = value; }
    }

    public MovementState state;
    public enum MovementState
    {
        idle,
        running,
        walking,
        wallrunning,
        sliding,
        dashing,
        jump,
        onWall,
        air,
        freeze,
        unlimited
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        GetInputs();
        SpeedControl();
        StateController();

        if (SlopeCheck())
        {
            _rb.drag = 20f;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        GroundCheck();
    }

    private void GetInputs()
    {
        _hInput = Input.GetAxisRaw("Horizontal");
        _vInput = Input.GetAxisRaw("Vertical");
        Input.GetKey(KeyCode.Z);

        if (Input.GetButton("Jump") && _canJump && _isGrounded)
        {
            _canJump = false;

            Jump();
            Invoke(nameof(ResetJump), _jumpCooldown);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            _isWalking = true;
        }
        else
        {
            _isWalking = false;
        }
    }

    private void StateController()
    {

        if (_isFreezed)
        {
            state = MovementState.freeze;
            _rb.velocity = Vector3.zero;

        }
        else if (_isUnlimited)
        {
            state = MovementState.unlimited;
            _movementSpeed = 999f;
            return;
        }
        else if (_isDashing)
        {
            state = MovementState.dashing;
            _movementSpeed = _dashSpeed;
        }
        else if (_isWallrunning)
        {
            state = MovementState.wallrunning;
            _movementSpeed = _wallRunSpeed;
            _rb.useGravity = false;
        }
        else if (_isSliding)
        {
            state = MovementState.sliding;
        }
        else if (_isOnWall)
        {
            state = MovementState.onWall;
        }
        else if (_isGrounded && IsMoving() && !_isWalking)
        {
            state = MovementState.running;
            _movementSpeed = _runSpeed;
        }
        else if (_isGrounded && IsMoving() && _isWalking)
        {
            state = MovementState.walking;
            _movementSpeed = _walkSpeed;
        }
        else if (_isGrounded)
        {
            state = MovementState.idle;
        }
        else
        {
            state = MovementState.air;
        }
    }


    private bool IsMoving()
    {
        if (_rb.velocity != new Vector3(0, 0, 0))
        {
            return true;
        }

        return false;
    }

    private void MovePlayer()
    {
        if (_isRestricted)
        {
            return;
        }

        _moveDirection = _player.forward * _vInput + _player.right * _hInput;

        if(SlopeCheck())
        {
            _rb.AddForce(SlopeMoveDirection(_moveDirection) * _movementSpeed * 10f, ForceMode.Force);

            if (_rb.velocity.y < 0)
                _rb.AddForce(Vector3.down * 100f, ForceMode.Force);
        }

        if(_isGrounded)
        {
            _rb.AddForce(_moveDirection.normalized * _movementSpeed * 10f, ForceMode.Force);
        }
        else if(!_isGrounded) 
        {
            _rb.AddForce(_moveDirection.normalized * _movementSpeed * _airMultiplayer, ForceMode.Force);
        }

        _rb.useGravity = !SlopeCheck(); 
    }

    private void GroundCheck()
    {
        RaycastHit hit;

        _isGrounded = Physics.SphereCast(transform.position, sphereRadius, Vector3.down, out hit, _playerHeight * 0.5f + 0.2f, _groundMask);

        if (_isGrounded)
        {
            _rb.drag = _groundDrag;
        }
        else
        {
            _rb.drag = 0f;
        }
    }


    private void SpeedControl()
    {
        if(SlopeCheck())
        {
            if(_rb.velocity.magnitude > _movementSpeed)
                _rb.velocity = _rb.velocity.normalized * _movementSpeed;
        }
        else
        {
            Vector3 flatVel = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

            if (flatVel.magnitude > _movementSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * _movementSpeed;
                _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        _canJump = true;
    }

    public bool SlopeCheck()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _playerHeight * 0.5f + 1f))
        {
            Debug.DrawLine(transform.position, transform.position + Vector3.down * (_playerHeight * 0.5f + 1f), Color.green);

            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);

            return angle < _maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 SlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, _slopeHit.normal).normalized;
    }
}
