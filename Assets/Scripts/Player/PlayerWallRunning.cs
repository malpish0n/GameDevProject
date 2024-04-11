using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallRunning : MonoBehaviour
{
    [SerializeField] private LayerMask _goundMask;
    [SerializeField] private LayerMask _wallMask;
    [SerializeField] private float _wallRunForce;
    [SerializeField] private float _maxWallRunTime;
    private float _wallRunTimer;

    private float _hInput, _vInput;

    [SerializeField] private float _wallCheckDistance;
    [SerializeField] private float _minJumpHeight;
    private RaycastHit _leftWallHit;
    private RaycastHit _rightWallHit;
    public bool _wallLeft;
    public bool _wallRight;

    [SerializeField] private Transform _playerOrientation;
    [SerializeField] private Transform _playerCamera;
    private PlayerMovement _movement;
    private Rigidbody _rb;

    private void Start()
    {
        GetReferences();
    }

    private void Update()
    {
        WallCheck();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (_movement._isWallrunning)
        {
            WallRunningMovement();
        }
    }

    private void WallCheck()
    {
        _wallRight = Physics.Raycast(transform.position, _playerOrientation.right, out _rightWallHit, _wallCheckDistance, _wallMask);
        Debug.DrawLine(transform.position, transform.position + _playerOrientation.right * _wallCheckDistance, Color.red);

        _wallLeft = Physics.Raycast(transform.position, -_playerOrientation.right, out _leftWallHit, _wallCheckDistance, _wallMask);
        Debug.DrawLine(transform.position, transform.position + -_playerOrientation.right * _wallCheckDistance, Color.red);
    }

    private bool IsAboveTheGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, _minJumpHeight, _goundMask);
    }

    private void StateMachine()
    {
        _hInput = Input.GetAxisRaw("Horizontal");
        _vInput = Input.GetAxisRaw("Vertical");

        // State 1 - WallRun
        if ((_wallLeft || _wallRight) && _vInput > 0 && IsAboveTheGround())
        {
            if(!_movement._isWallrunning)
            {
                StartWallRun();
            }
        }
        else
        {
            if(_movement._isWallrunning)
            {
                StopWallRun();
            }
        }
    }

    private void StartWallRun()
    {
        _movement._isWallrunning = true;
    }

    private void WallRunningMovement()
    {
        _rb.useGravity = false;
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        Vector3 wallNormal = _wallRight ? _rightWallHit.normal : _leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if (Input.GetKey(KeyCode.Space))
        {
            _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
            _rb.AddForce(transform.up * 3f + wallNormal * 5f, ForceMode.Impulse);
        }
    }

    private void StopWallRun()
    {
        _rb.useGravity = true;
        _movement._isWallrunning = false;
    }

    private void GetReferences()
    {
        _rb = GetComponent<Rigidbody>();
        _movement = GetComponent<PlayerMovement>();
    }
}
