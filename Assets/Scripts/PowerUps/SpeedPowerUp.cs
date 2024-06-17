using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private float _multiplier;
    [SerializeField] private PlayerMovement _playerMovement;
    private float _defaultRunSpeed;

    private void Start()
    {
        _defaultRunSpeed = _playerMovement._runSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _playerMovement._runSpeed *= _multiplier;

            Invoke("ResetSpeed", _duration);
        }

    }

    private void ResetSpeed()
    {
        _playerMovement._runSpeed = _defaultRunSpeed;
    }
}
