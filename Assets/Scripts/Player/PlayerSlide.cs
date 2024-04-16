using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : MonoBehaviour
{
    [SerializeField] private Transform _playerOrientation;
    [SerializeField] private Transform _playerModel;
    private Rigidbody _rb;
    private PlayerMovement _playerMovement;
}
