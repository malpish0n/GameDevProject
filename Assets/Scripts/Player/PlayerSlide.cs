using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.Tilemaps.Tilemap;

public class PlayerSlide : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to whole player object")]
    [SerializeField] private Transform _playerModel;
    [Tooltip("Reference to player collider")]
    [SerializeField] private Transform _playerCollider;
    private Rigidbody _rb;
    private PlayerMovement _playerMovement;

    [Header("Slide variables")]
    [SerializeField] private float _maxSlideTime;
    [SerializeField] private float _slideForce;

    [Header("Timers")]
    private float _slideTimer;

    [Header("Scale variabless")]
    [SerializeField] private float _slideYScale;
    private float _startYScale;

    [Header("Inputs")]
    private float _vInput, _hInput;

    [Header("State bools")]
    private bool _isSliding;
    
    private void Start()
    {
        GetRefereneces();
    }

    private void Update()
    {
        GetInputs();
    }

    private void FixedUpdate()
    {
        if (_isSliding)
        {
            SlidingMovement();
        }
    }

    private void GetInputs()
    {
        _hInput = Input.GetAxisRaw("Horizontal");
        _vInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftControl) && (_hInput !=0 || _vInput != 0) && _playerMovement._isGrounded)
        {
            StartSlide();
        }

        if(Input.GetKeyUp(KeyCode.LeftControl) && _isSliding)
        {
            StopSlide();
        }
    }

    private void StartSlide()
    {
        _playerMovement.IsSliding = true;
        _isSliding = true;
        _playerCollider.localScale = new Vector3(_playerCollider.localScale.x, _slideYScale, _playerCollider.localScale.z);
        _rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        _slideTimer = _maxSlideTime;
    }
    private void SlidingMovement()
    {
        Vector3 inputDirection = _playerModel.forward * _vInput + _playerModel.right * _hInput;
        
        //normal sliding
        if(!_playerMovement.SlopeCheck() || _rb.velocity.y > -0.01f)
        {
            _rb.AddForce(inputDirection.normalized * _slideForce, ForceMode.Force);
            _slideTimer -= Time.deltaTime;
        }
        //sliding down the slope
        else
        {
            _rb.AddForce(_playerMovement.SlopeMoveDirection(inputDirection) * _slideForce, ForceMode.Force);
        }


        if(_slideTimer <= 0)
        {
            StopSlide();
        }
    }

    private void StopSlide()
    {
        _playerMovement.IsSliding = false;
        _isSliding = false;
        _playerCollider.localScale = new Vector3(_playerCollider.localScale.x, _startYScale, _playerCollider.localScale.z);
    }

    private void GetRefereneces()
    {
        _rb = GetComponent<Rigidbody>();
        _playerMovement = GetComponent<PlayerMovement>();

        _startYScale = _playerModel.localScale.y;
    }
}
