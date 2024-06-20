using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrabbing : MonoBehaviour
{
    [Header("Refs")]
    [Tooltip("Reference to whole player object")]
    [SerializeField] Transform _player;
    [Tooltip("Reference to camera game object")]
    [SerializeField] private Transform _playerCamera;
    [SerializeField] PlayerMovement _movement;
    private Rigidbody _rigidbody;

    [Header("Raycasts")]
    [SerializeField] private float _detectLength;
    [SerializeField] private float _sphereRadius;
    [SerializeField] private LayerMask _ledgeMask;

    private Transform _lastLedge;
    private Transform _currentLedge;

    private RaycastHit _ledgeHit;

    [Header("Ledge Grab")]
    [SerializeField] private float _moveToLedgeSpeed;
    [SerializeField] private float _maxGrabDistance;
    [SerializeField] private float _minTimeOnLedge;
    private float _timeOnLedge;

    public bool _isOnLedge;

    [Header("Ledge Jumping")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private float _ledgeJumpForwardForce;
    [SerializeField] private float _ledgeJumpUpwardForce;

    [Header("Exiting")]
    public bool _exitingLedge;
    public float _exitLedgeTime;
    private float _exitLedgeTimer;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        LedgeDetect();
        StateMachine();
    }

    private void StateMachine()
    {
        float _hInput = Input.GetAxisRaw("Horizontal");
        float _vInput = Input.GetAxisRaw("Vertical");
        bool _keyPressed = _hInput != 0 || _vInput != 0;

        if (_isOnLedge)
        {
            FreezeRigidbodyOnLedge();

            _timeOnLedge += Time.deltaTime;

            if (_timeOnLedge > _minTimeOnLedge && _keyPressed)
            {
                ExitLedgeHold();
            }

            if (Input.GetKeyDown(_jumpKey))
            {
                LedgeJump();
            }
        }
        else if (_exitingLedge)
        {
            if (_exitLedgeTimer > 0)
            {
                _exitLedgeTimer -= Time.deltaTime;
            }
            else
            {
                _exitingLedge = false;
            }
        }
    }

    private void LedgeDetect()
    {
        bool ledgeDetected = Physics.SphereCast(transform.position, _sphereRadius, _playerCamera.forward, out _ledgeHit, _detectLength, _ledgeMask);

        if (!ledgeDetected)
        {
            return;
        }

        float distanceToLedge = Vector3.Distance(transform.position, _ledgeHit.transform.position);

        if (_ledgeHit.transform == _lastLedge)
        {
            return;
        }

        if (distanceToLedge < _maxGrabDistance && !_isOnLedge)
        {
            EnterLedge();
        }
    }

    private void EnterLedge()
    {
        _isOnLedge = true;

        _movement._isUnlimited = true;
        _movement._isRestricted = true;

        _currentLedge = _ledgeHit.transform;
        _lastLedge = _ledgeHit.transform;

        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
    }

    private void FreezeRigidbodyOnLedge()
    {
        _rigidbody.useGravity = false;

        Vector3 ledgeDirection = _currentLedge.position - transform.position;
        float distanceToLedge = Vector3.Distance(transform.position, _currentLedge.position);

        if (distanceToLedge > 1f)
        {
            if (_rigidbody.velocity.magnitude < _moveToLedgeSpeed)
            {
                _rigidbody.AddForce(ledgeDirection.normalized * _moveToLedgeSpeed * 1000f * Time.deltaTime);
            }
            else
            {
                if (!_movement._isFreezed)
                {
                    _movement._isFreezed = true;
                }
                if (_movement._isUnlimited)
                {
                    _movement._isUnlimited = false;
                }
            }

            if (distanceToLedge > _maxGrabDistance)
            {
                ExitLedgeHold();
            }
        }
    }

    private void LedgeJump()
    {
        ExitLedgeHold();
        Invoke(nameof(DelayedJumpForce), 0.05f);
    }

    private void DelayedJumpForce()
    {
        Vector3 forceToAdd = _playerCamera.forward * _ledgeJumpForwardForce + Vector3.up * _ledgeJumpUpwardForce;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddForce(forceToAdd, ForceMode.Impulse);
    }

    private void ExitLedgeHold()
    {
        _exitingLedge = true;
        _exitLedgeTimer = _exitLedgeTime;

        _isOnLedge = false;
        _timeOnLedge = 0;

        _movement._isRestricted = false;
        _movement._isFreezed = false;

        _rigidbody.useGravity = true;

        StopAllCoroutines();
        Invoke("ResetLastLedge", 1f);
    }

    private void ResetLastLedge()
    {
        _lastLedge = null;
    }
}
