using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWallJump : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to whole player object")]
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _camera;
    private Rigidbody _rb;
    private PlayerMovement _playerMovement;

    [Header("Raycasts")]
    private RaycastHit _wallRaycastHit;
    private float _wallCheckDistance = .6f;
    [Tooltip("Layer mask of wall that could be use for wall jumping")]
    [SerializeField] private LayerMask _wallMask;
    [SerializeField] private LayerMask _groundMask;

    [Header("State bools")]
    private bool _isOnWall;
    private bool _exitWall;
    private bool isJumping = false;

    [Header("Wall Slide Settings")]
    [SerializeField] private float wallSlideSpeed;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        WallCheck();
        GroundCheck();
        WallJump();
        
        _isOnWall = GroundCheck() && WallCheck();
        _playerMovement.IsOnWall = _isOnWall;
    }

    private bool WallCheck()
    {
        Debug.DrawRay(transform.position, _player.forward * _wallCheckDistance, Color.red);

        return Physics.Raycast(transform.position, _player.forward, out _wallRaycastHit, _wallCheckDistance, _wallMask) ? true : false;
    }

    private bool GroundCheck()
    {
        return !Physics.Raycast(transform.position, Vector3.down, 1.1f, _groundMask);
    }

    private void WallJump()
    {
        if (_isOnWall && !isJumping)
        {
            _rb.useGravity = false;
            _rb.velocity = new Vector3(_rb.velocity.x, -wallSlideSpeed, _rb.velocity.z);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
    }

    private void Jump()
    {
        isJumping = true;
        _rb.useGravity = true;
        _isOnWall = false;

        Vector3 forceToApply = transform.forward * -11f + transform.up * 11f;
        _rb.AddForce(forceToApply, ForceMode.Impulse);

        Invoke("ResetJumping", 0.2f);
    }

    private void ResetJumping()
    {
        isJumping = false;
    }
}
