using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallRunning : MonoBehaviour
{
    [SerializeField] private LayerMask _goundMask;
    [SerializeField] private LayerMask _wallMask;
    [SerializeField] private float _wallRunForce;
    [SerializeField] private float _maxWallRunTime;
    [SerializeField] private float _wallRunTimer;

    private float _hInput, _vInput;

    [SerializeField] private float _wallCheckDistance;
    [SerializeField] private float _minJumpHeigght;
    private RaycastHit _leftWallHit;
    private RaycastHit _rightWallHit;
    private bool _wallLeft;
    private bool _wallRight;

    [SerializeField] private Transform _playerOreintation;
    private PlayerMovement _movement;
    private Rigidbody _rb;

    private void Start()
    {
        GetReferences();
    }

    private void Update()
    {
        WallCheck();
    }

    private void WallCheck()
    {
        _wallRight = Physics.Raycast(transform.position, _playerOreintation.right, out _rightWallHit, _wallCheckDistance, _wallMask);
        _wallLeft = Physics.Raycast(transform.position, -_playerOreintation.right, out _leftWallHit, _wallCheckDistance, _wallMask);
    }

    private bool IsAboveTheGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, _minJumpHeigght, _goundMask);
    }

    private void StateMachine()
    {
        _hInput = Input.GetAxisRaw("Horizontal");
        _vInput = Input.GetAxisRaw("Vertical");

        // State 1 - WallRun
        if ((_wallLeft || _wallRight) && _vInput > 0 && IsAboveTheGround())
        {

        }
    }

    private void StartWallRun()
    {

    }

    private void WallRunningMovement()
    {

    }

    private void StopWallRun()
    {

    }

    private void GetReferences()
    {
        _rb = GetComponent<Rigidbody>();
        _movement = GetComponent<PlayerMovement>();
    }
}
