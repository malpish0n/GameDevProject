using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to whole player object")]
    [SerializeField] private Transform _player;
    [Tooltip("Reference to camera game object")]
    [SerializeField] private Camera _playerCamera;
    private Rigidbody _rb;
    private PlayerMovement _playerMovement;
    private PlayerStats _playerStats;

    [Header("Camera variables")]
    private float _basicCameraFov;
    private float _targetCameraFov = 70f;

    [Header("Dash variables")]
    [SerializeField] private float _dashForce;
    [SerializeField] private float _dashUpwardForce;
    [SerializeField] private float _dashDuration;
    [SerializeField] private int _dashStaminaCost;

    [Header("Timers")]
    [Tooltip("Dash coldown")]
    [SerializeField] private float _dashCd;
    private float _dashCdTimer;
    private float _cameraFovTimer = 2f;
    private float _currentCameraFovTime;

    [Header("State bools")]
    private bool _isDashing = false;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerStats = GetComponent<PlayerStats>();
        _basicCameraFov = _playerCamera.fieldOfView;
    }

    private void Update()
    {
        GetInputs();
    }

    private void GetInputs()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _rb.velocity != Vector3.zero && _playerStats._stamina > 0 && !_isDashing)
        {
            StartCoroutine(DashCoroutine());
        }

        if (_dashCdTimer > 0)
        {
            _dashCdTimer -= Time.deltaTime;
        }
    }

    private IEnumerator DashCoroutine()
    {
        if (_dashCdTimer > 0)
        {
            yield break;
        }
        else
        {
            _dashCdTimer = _dashCd;
        }

        _playerMovement.IsDashing = true;
        _playerStats.loseStamina(_dashStaminaCost);

        Vector3 forceToApply = _playerCamera.transform.forward * _dashForce + _playerCamera.transform.up * _dashUpwardForce;

        _rb.AddForce(forceToApply, ForceMode.Impulse);

        float timer = 0f;
        _isDashing = true;

        while (timer < _dashDuration)
        {
            _playerCamera.fieldOfView = Mathf.Lerp(_playerCamera.fieldOfView, _targetCameraFov, timer / _dashDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(_dashDuration);

        timer = 0f;

        while (timer < _dashDuration)
        {
            _playerCamera.fieldOfView = Mathf.Lerp(_playerCamera.fieldOfView, _basicCameraFov, timer / _dashDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        _isDashing = false;
        _playerMovement.IsDashing = false;
    }
}
