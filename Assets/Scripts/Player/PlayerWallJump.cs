using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJump : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _playerModel;
    private Rigidbody _rb;
    private PlayerMovement _playerMovement;

    [Header("Raycasts")]
    private RaycastHit _wallRaycastHit;
    private float _wallCheckDistance = 0.60f;
    [Tooltip("Layer mask of wall that could be use for wall jumping")]
    [SerializeField] private LayerMask _wallMask;

    private bool _isOnWall = false;
    private bool _exitWall;

    void Start()
    {
        GetReferences();
    }

    void Update()
    {
        WallCheck();
        GroundCheck();
        WallJump();

        Debug.Log("Is on wall: " +  _isOnWall);
    }

    private bool WallCheck()
    {
        Debug.DrawRay(transform.position, _playerModel.forward * _wallCheckDistance, Color.red);


        if (Physics.Raycast(transform.position, _playerModel.forward, out _wallRaycastHit, _wallCheckDistance, _wallMask))
        {
            return true;
        }

        return false;
    }

    private bool GroundCheck()
    {
        if(_playerMovement.state == PlayerMovement.MovementState.air)
        {
            return true;
        }
        
        return false;
    }

    private void WallJump()
    {
        _isOnWall = WallCheck() && GroundCheck();

        if (_isOnWall)
        {
            _rb.useGravity = false;
            _rb.velocity = Vector3.zero;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        else
        {
            _rb.useGravity = true;
        }
    }


    private void Jump()
    {
        _rb.useGravity = true;
        _isOnWall = false;

        Vector3 forceToApply = transform.forward * -10f + transform.up * 10f;
        _rb.AddForce(forceToApply, ForceMode.Impulse);
        _playerModel.transform.rotation = Quaternion.LookRotation(transform.forward * -1);
    }

    private void GetReferences()
    {
        _rb = GetComponent<Rigidbody>();
        _playerMovement = GetComponent<PlayerMovement>();
    }
}
