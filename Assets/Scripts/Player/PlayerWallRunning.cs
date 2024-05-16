using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallRunning : MonoBehaviour
{
    [SerializeField] private LayerMask _goundMask;
    [SerializeField] private LayerMask _wallMask;

    [SerializeField] private float _wallRunForce;
    [SerializeField] private float _wallJumpUpForce;
    [SerializeField] private float _wallJumpSideForce;
    [SerializeField] private float _maxWallRunTime;
    private float _wallRunTimer;

    private float _hInput, _vInput;

    [SerializeField] private float _wallCheckDistance;
    [SerializeField] private float _minJumpHeight;
    private RaycastHit _leftWallHit;
    private RaycastHit _rightWallHit;
    public bool _wallLeft;
    public bool _wallRight;

    private bool _exitWall;
    [SerializeField] private float _exitWallTime;
    private float _exitWallTimer;

    [SerializeField] private Transform _playerModel;
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
        _wallRight = Physics.Raycast(transform.position, _playerModel.right, out _rightWallHit, _wallCheckDistance, _wallMask);
        Debug.DrawLine(transform.position, transform.position + _playerModel.right * _wallCheckDistance, Color.red);

        _wallLeft = Physics.Raycast(transform.position, -_playerModel.right, out _leftWallHit, _wallCheckDistance, _wallMask);
        Debug.DrawLine(transform.position, transform.position + -_playerModel.right * _wallCheckDistance, Color.red);
    }

    private bool IsAboveTheGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, _minJumpHeight, _goundMask);
    }

    private void StateMachine()
    {
        _hInput = Input.GetAxisRaw("Horizontal");
        _vInput = Input.GetAxisRaw("Vertical");

        if ((_wallLeft || _wallRight) && _vInput > 0 && IsAboveTheGround() && !_exitWall)
        {
            if(!_movement._isWallrunning)
            {
                StartWallRun();
            }

            if (_wallRunTimer > 0)
            {
                _wallRunTimer -= Time.deltaTime;
            }

            if (_wallRunTimer <= 0 && _movement._isWallrunning)
            {
                _exitWall = true;
                _exitWallTimer = _exitWallTime;
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                WallJump();
            }
        }
        else if (_exitWall)
        {
            if (_movement._isWallrunning)
            {
                StopWallRun();
            }

            if(_exitWallTimer > 0)
            {
                _exitWallTimer -= Time.deltaTime;
            }

            if(_exitWallTimer <= 0)
            {
                _exitWall = false;
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
        _wallRunTimer = _maxWallRunTime;
    }

    private void WallRunningMovement()
    {
        _rb.useGravity = false;
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        Vector3 wallNormal = _wallRight ? _rightWallHit.normal : _leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);
    }

    private void StopWallRun()
    {
        _rb.useGravity = true;
        _movement._isWallrunning = false;
    }

    private void WallJump()
    {
        _exitWall = true;
        _exitWallTimer = _exitWallTime;

        Vector3 wallNormal = _wallRight ? _rightWallHit.normal : _leftWallHit.normal;

        Vector3 forceToApply = transform.up * _wallJumpUpForce + wallNormal * _wallJumpSideForce;

        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        _rb.AddForce(forceToApply, ForceMode.Impulse);
    }

    private void GetReferences()
    {
        _rb = GetComponent<Rigidbody>();
        _movement = GetComponent<PlayerMovement>();
    }
}
