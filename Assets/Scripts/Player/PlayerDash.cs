using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private Transform _playerModel;
    [SerializeField] private Camera _playerCamera;
    private Rigidbody _rb;
    private PlayerMovement _playerMovement;
    private PlayerStats _playerStats;
    private float _cameraFov = 60f;

    [SerializeField] private float _dashForce;
    [SerializeField] private float _dashUpwardForce;
    [SerializeField] private float _dashDuration;
    [SerializeField] private int _dashStaminaCost;

    [SerializeField] private float _dashCd;
    private float _dashCdTimer;

    private void Start()
    {
        GetReferences();
    }

    private void Update()
    {
        GetInputs();
    }

    private void GetInputs()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _rb.velocity != new Vector3(0, 0, 0) && _playerStats._stamina > 0)
        {
            Dash();
        }

        if(_dashCdTimer > 0)
        {
            _dashCdTimer -= Time.deltaTime;
        }
    }

    private void Dash()
    {
        if(_dashCdTimer > 0)
        {
            return;
        }
        else
        {
            _dashCdTimer = _dashCd;
        }
        
        _playerMovement.IsDashing = true;
        _playerStats.loseStamina(_dashStaminaCost);

        Vector3 forceToApply = _playerCamera.transform.forward * _dashForce + _playerCamera.transform.up * _dashUpwardForce;

        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(ResetDash), _dashDuration);
    }

    private Vector3 delayedForceToApply;

    private void DelayedDashForce()
    {
        _rb.AddForce(delayedForceToApply, ForceMode.Impulse);   
    }

    private void ResetDash()
    {
        _playerMovement.IsDashing = false;
    }

    private void GetReferences()
    {
        _rb = GetComponent<Rigidbody>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerStats = GetComponent<PlayerStats>();
    }
}
