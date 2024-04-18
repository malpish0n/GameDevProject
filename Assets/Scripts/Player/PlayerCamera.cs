using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float _sensitivity;
    [SerializeField] private Transform _playerOrientation;
    [SerializeField] private PlayerWallRunning _playerWallRunning;

    private float _xRotation, _yRotation;
    private float _mouseX, _mouseY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        GetInputs();
        MoveCamera();
        WallrunCamRotate();
    }

    private void GetInputs()
    {
        _mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * _sensitivity;
        _mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * _sensitivity;
    }

    private void MoveCamera()
    {
        _yRotation += _mouseX;
        _xRotation -= _mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        _playerOrientation.rotation = Quaternion.Euler(0, _yRotation, 0);
    }

    private void WallrunCamRotate()
    {
        if (_playerWallRunning._wallRight)
        {
            transform.rotation *= Quaternion.Euler(0, 0, 10f);
        }
        else if (_playerWallRunning._wallLeft)
        {
            transform.rotation *= Quaternion.Euler(0, 0, -10f);
        }
    }
}
