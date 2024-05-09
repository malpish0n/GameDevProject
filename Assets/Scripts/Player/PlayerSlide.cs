using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.Tilemaps.Tilemap;

public class PlayerSlide : MonoBehaviour
{
    [SerializeField] private Transform _playerOrientation;
    [SerializeField] private Transform _playerModel;
    private Rigidbody _rb;
    private PlayerMovement _playerMovement;

    [SerializeField] private float _maxSlideTime;
    [SerializeField] private float _slideForce;
    private float _slideTimer;

    [SerializeField] private float _slideYScale;
    private float _startYScale;

    private float _vInput, _hInput;

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
        _playerModel.localScale = new Vector3(_playerModel.localScale.x, _slideYScale, _playerModel.localScale.z);
        _rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        _slideTimer = _maxSlideTime;
    }
    private void SlidingMovement()
    {
        Vector3 inputDirection = _playerOrientation.forward * _vInput + _playerOrientation.right * _hInput;
        
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
        _playerModel.localScale = new Vector3(_playerModel.localScale.x, _startYScale, _playerModel.localScale.z);
    }

    private void GetRefereneces()
    {
        _rb = GetComponent<Rigidbody>();
        _playerMovement = GetComponent<PlayerMovement>();

        _startYScale = _playerModel.localScale.y;
    }
}
