using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    public float _walkSpeed;
    public float _runSpeed;
    public float _wallRunSpeed;
    public float _dashSpeed;

    [SerializeField] private Transform _playerModel;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _airMultiplayer;
    private bool _canJump = true;

    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _groundDrag;

    [SerializeField] private float _maxSlopeAngle;
    private RaycastHit _slopeHit;

    public float _hInput, _vInput;
    private Vector3 _moveDirection;

    private Rigidbody _rb;
    private Animator _animator;

    public bool _isGrounded;
    public bool _isRunning;
    public bool _isWalking;
    public bool _isWallrunning;
    private bool _isSliding;
    private bool _isDashing;

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
        air
    }

    private void Start()
    {
        GetReferences();
    }

    private void Update()
    {
        GetInputs();
        SpeedControl();
        StateController();
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
        if (_isGrounded && IsMoving() && !_isWalking)
        {
            state = MovementState.running;
            _movementSpeed = _runSpeed;
        }
        else if (_isGrounded && IsMoving() && _isWalking) 
        { 
            state = MovementState.walking;
            _movementSpeed = _walkSpeed;
        }
        else
        {
            state = MovementState.idle;
        }

        if (_isSliding)
        {
            state = MovementState.sliding;
        }

        if (_isDashing || (!_isGrounded && _isDashing))
        {
            state = MovementState.dashing;
            _movementSpeed = _dashSpeed;
        }
        else if (!_isGrounded)
        {
            state = MovementState.air;
        }

        if (_isWallrunning)
        {
            state = MovementState.wallrunning;
            _movementSpeed = _wallRunSpeed;
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
        _moveDirection = _playerModel.forward * _vInput + _playerModel.right * _hInput;

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
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _groundMask);
        Debug.DrawLine(transform.position, transform.position - Vector3.up * (_playerHeight * 0.5f + 0.2f), Color.red);

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

    private void GetReferences()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }
}
