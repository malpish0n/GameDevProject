using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private Transform _playerOrientation;
    [SerializeField] private Transform _playerModel;
    private Rigidbody _rb;
    private PlayerMovement _playerMovement;

    [SerializeField] private float _maxDashTime;
    [SerializeField] private float _dashForce;
    private float _dashTimer;

    private float _vInput, _hInput;

    private bool _isDashing;

    private void Start()
    {
        GetReferences();
    }

    private void Update()
    {
        GetInputs();
    }

    private void FixedUpdate()
    {
        if (_isDashing)
        {
            Dash();
        }
    }

    private void GetInputs()
    {
        _hInput = Input.GetAxisRaw("Horizontal");
        _vInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift) && (_hInput != 0 || _vInput != 0))
        {
            StartDash();
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) && _isDashing)
        {
            StopDash();
        }
    }

    private void StartDash()
    {
        _playerMovement._isDashing = true;
        _isDashing = true;
        _dashTimer = _maxDashTime;
    }

    private void Dash()
    {
        Vector3 inputDirection = _playerOrientation.forward * _vInput + _playerOrientation.right * _hInput;

        _rb.AddForce(inputDirection.normalized * _dashForce, ForceMode.Force);
        _dashTimer -= Time.deltaTime;

        if (_dashTimer <= 0)
        {
            StopDash();
        }
    }

    private void StopDash()
    {
        _playerMovement._isDashing = false;
        _isDashing = false;
    }

    private void GetReferences()
    {
        _rb = GetComponent<Rigidbody>();
        _playerMovement = GetComponent<PlayerMovement>();
    }
}
