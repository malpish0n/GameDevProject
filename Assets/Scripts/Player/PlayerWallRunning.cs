using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallRunning : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to whole player object")]
    [SerializeField] private Transform _player;
    //[Tooltip("Reference to camera game object")]
    //[SerializeField] private Transform _playerCamera;
    private PlayerMovement _movement;
    private Rigidbody _rb;

    [Header("Forces")]
    [SerializeField] private float _wallRunForce;
    [SerializeField] private float _wallJumpUpForce;
    [SerializeField] private float _wallJumpSideForce;

    [Header("Timer")]
    [SerializeField] private float _maxWallRunTime;
    private float _wallRunTimer;

    [Header("Inputs")]
    private float _hInput, _vInput;

    [Header("Raycasts")]
    [SerializeField] private float _wallCheckDistance;
    [SerializeField] private float _minJumpHeight;
    [SerializeField] private LayerMask _goundMask;
    [SerializeField] private LayerMask _wallMask;
    private RaycastHit _leftWallHit;
    private RaycastHit _rightWallHit;
    public bool _wallLeft;
    public bool _wallRight;

    [Header("Wall exiting")]
    private bool _exitWall;
    [SerializeField] private float _exitWallTime;
    private float _exitWallTimer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _movement = GetComponent<PlayerMovement>();
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
        _wallRight = Physics.Raycast(transform.position, _player.right, out _rightWallHit, _wallCheckDistance, _wallMask);
        Debug.DrawLine(transform.position, transform.position + _player.right * _wallCheckDistance, Color.red);

        _wallLeft = Physics.Raycast(transform.position, -_player.right, out _leftWallHit, _wallCheckDistance, _wallMask);
        Debug.DrawLine(transform.position, transform.position + -_player.right * _wallCheckDistance, Color.red);
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
            if (!_movement._isWallrunning)
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

            if (Input.GetKeyDown(KeyCode.Space))
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

            if (_exitWallTimer > 0)
            {
                _exitWallTimer -= Time.deltaTime;
            }

            if (_exitWallTimer <= 0)
            {
                _exitWall = false;
            }
        }
        else
        {
            if (_movement._isWallrunning)
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

        Vector3 wallNormal = _wallRight ? _rightWallHit.normal : _leftWallHit.normal;
        Vector3 wallForward;

        if (_wallRight)
        {
            wallForward = Vector3.Cross(transform.up, wallNormal); // Odwrócony kierunek
        }
        else
        {
            wallForward = Vector3.Cross(wallNormal, transform.up);
        }

        // Usuniêcie sk³adowej pionowej prêdkoœci
        Vector3 velocity = _rb.velocity;
        velocity.y = 0;
        _rb.velocity = velocity;

        // Dodanie si³y w kierunku œciany (wzd³u¿ œciany)
        _rb.AddForce(wallForward * _wallRunForce, ForceMode.Force);

        // Jeœli potrzebujesz kompensowaæ grawitacjê, dodaj si³ê w górê
        _rb.AddForce(Vector3.up * (_wallRunForce / 5), ForceMode.Force);  // Adjust the division factor as needed
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
}