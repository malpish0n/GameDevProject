using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private float _hInput, _vInput;
    private Vector3 _moveDirection;

    private Rigidbody _rb;

    public MovementState state;

    public enum MovementState
    {
        running,
        walking,
        wallrunning,
        air
    }

    public bool _isWallrunning;

    private void Start()
    {
        GetReferences();
    }

    private void Update()
    {
        Debug.Log("Wall run speed: " + _wallRunSpeed);

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
        if(_isWallrunning)
        {
            state = MovementState.wallrunning;
            _movementSpeed = _wallRunSpeed;
        }

        if(_isGrounded) 
        {
            state = MovementState.running;
            _movementSpeed = _runSpeed;    
        }
        else if (_isGrounded && Input.GetKey(KeyCode.LeftAlt))
        {
            state = MovementState.walking;
            _movementSpeed = _walkSpeed;
        }
        else
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        _moveDirection = orientation.forward * _vInput + orientation.right * _hInput;

        if(_isGrounded )
        {
            _rb.AddForce(_moveDirection.normalized * _movementSpeed * 10f, ForceMode.Force);
        }
        else if(!_isGrounded) 
        {
            _rb.AddForce(_moveDirection.normalized * _movementSpeed * _airMultiplayer, ForceMode.Force);
        }
        
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
        Vector3 flatVel = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > _movementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * _movementSpeed;
            _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
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

    private void GetReferences()
    {
        _rb = GetComponent<Rigidbody>();
    }
}
