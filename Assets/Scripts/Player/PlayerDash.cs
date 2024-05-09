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
    
    private float _basicCameraFov;
    private float _targetCameraFov = 80f;
    private float _cameraFovTimer = 2f;
    private float _currentCameraFovTime;

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
        if (Input.GetKey(KeyCode.LeftShift) && _rb.velocity != new Vector3(0, 0, 0) && _playerStats._stamina > 0)
        {
            Dash();
            ChangeCameraFove();
        }
        else
        {
            ResetCameraFov();
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

    private void ChangeCameraFove()
    {
        _playerCamera.fieldOfView = Mathf.Lerp(_playerCamera.fieldOfView, _targetCameraFov, 10f * Time.deltaTime);
    }

    private void ResetCameraFov()
    {
        _playerCamera.fieldOfView = Mathf.Lerp(_playerCamera.fieldOfView, _basicCameraFov, 10f * Time.deltaTime);
    }

    IEnumerator ChangeCameraFov()
    {
        _currentCameraFovTime = _cameraFovTimer;

        while(_currentCameraFovTime > 0f)
        {
            _currentCameraFovTime -= Time.deltaTime;
            _playerCamera.fieldOfView = _targetCameraFov;

            yield return null;
        }
    }

    private void GetReferences()
    {
        _rb = GetComponent<Rigidbody>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerStats = GetComponent<PlayerStats>();
        _basicCameraFov = _playerCamera.fieldOfView;
    }
}
