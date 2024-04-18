using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    public float _walkSpeed;
    public float _runSpeed;

    [SerializeField] private Transform orientation;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _airMultiplayer;
    private bool _canJump = true;

    public float _wallRunSpeed;

    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundMask;
    private bool _isGrounded;
    [SerializeField] private float _groundDrag;

    [SerializeField] private float _maxSlopeAngle;
    [SerializeField] private LayerMask _slopeMask;
    private RaycastHit _slopeHit;

    private float _hInput, _vInput;
    private Vector3 _moveDirection;

    private Rigidbody _rb;
    private Animator _animator;

    public MovementState state;

    public enum MovementState
    {
        running,
        walking,
        wallrunning,
        sliding,
        dashing,
        idle,
        air
    }

    public bool _isWallrunning;
    public bool _isSliding;
    public bool _isDashing;

    private void Start()
    {
        GetReferences();
    }

    private void Update()
    {
        Debug.Log(_rb.velocity);

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

        if (Input.GetKey(KeyCode.Space) && _canJump && _isGrounded)
        {
            _canJump = false;

            Jump();
            Invoke(nameof(ResetJump), _jumpCooldown);
        }
    }

    private void StateController()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            state = MovementState.walking;
            _animator.SetBool("isFalling", false);
            _animator.SetFloat("Speed", 0.5f, 0.2f, Time.deltaTime);
            _movementSpeed = _walkSpeed;
        }
        else if (_isWallrunning)
        {
            state = MovementState.wallrunning;
            _movementSpeed = _wallRunSpeed;
        }
        else if (_isGrounded && (_rb.velocity.x != 0 || _rb.velocity.y != 0 || _rb.velocity.z != 0))
        {
            state = MovementState.running;
            _animator.SetBool("isFalling", false);
            _animator.SetFloat("Speed", 1f, 0.2f, Time.deltaTime);
            _movementSpeed = _runSpeed;
        }
        else if (_isSliding)
        {
            state = MovementState.sliding;
        }
        else if (_isDashing)
        {
            state = MovementState.dashing;
        }
        else if (_isGrounded && (_rb.velocity.x == 0 || _rb.velocity.y == 0 || _rb.velocity.z == 0))
        {
            state = MovementState.idle;
            _animator.SetBool("isFalling", false);
            _animator.SetFloat("Speed", 0, 0.2f, Time.deltaTime);
        }
        else
        {
            state = MovementState.air;
            _animator.SetBool("isFalling", true);
        }
    }

    private void MovePlayer()
    {
        _moveDirection = orientation.forward * _vInput + orientation.right * _hInput;

        if(SlopeCheck())
        {
            _rb.AddForce(SlopeMoveDirection(_moveDirection) * _movementSpeed * 10f, ForceMode.Force);

            if (_rb.velocity.y < 0)
                _rb.AddForce(Vector3.down * 100f, ForceMode.Force);
        }

        if(_isGrounded )
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

            // limit velocity if needed
            if (flatVel.magnitude > _movementSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * _movementSpeed;
                _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        // reset y velocity
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
