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
    private PlayerCamera _playerCamera;
    private Rigidbody _rb;
    private PlayerMovement _playerMovement;

    [Header("Raycasts")]
    private RaycastHit _wallRaycastHit;
    private float _wallCheckDistance = .6f;
    [Tooltip("Layer mask of wall that could be use for wall jumping")]
    [SerializeField] private LayerMask _wallMask;

    [SerializeField] private bool _isOnWall;
    private bool _exitWall;
    private bool isJumping = false;

    [Header("Wall Slide Settings")]
    [SerializeField] private float wallSlideSpeed = .05f;

    void Start()
    {
        GetReferences();
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

        if (Physics.Raycast(transform.position, _player.forward, out _wallRaycastHit, _wallCheckDistance, _wallMask))
        {
            return true;
        }

        return false;
    }

    private bool GroundCheck()
    {
        if (_playerMovement.state == PlayerMovement.MovementState.air)
        {
            return true;
        }

        return false;
    }

    //Dodaæ korutynê, która bêdzie w³¹czaæ i wy³¹czaæ skrypt odpowiedzialny za obrót kamery
    // Poogarniaæ wszustkie skrypty: nag³ówki i tooltpiy
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
        else
        {
            if (!isJumping)
            {
                _rb.useGravity = true;
            }
            
        }
    }

    private void Jump()
    {
        isJumping = true;
        _rb.useGravity = true;
        _isOnWall = false;

        Vector3 forceToApply = transform.forward * -10f + transform.up * 10f;
        _rb.AddForce(forceToApply, ForceMode.Impulse);

        _player.transform.rotation = Quaternion.LookRotation(transform.forward * -1);
        _camera.transform.rotation = Quaternion.LookRotation(transform.forward * -1);

        Invoke(nameof(ResetJumping), 0.2f);
    }

    private void ResetJumping()
    {
        isJumping = false;
    }

    private void GetReferences()
    {
        _rb = GetComponent<Rigidbody>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerCamera = _camera.GetComponent<PlayerCamera>();
    }
}
