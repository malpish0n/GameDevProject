using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVaulting : MonoBehaviour
{
    [SerializeField] private LayerMask _vaultLayer;
    [SerializeField] private Transform _playerModel;
    private Rigidbody _rb;
    private bool _canVault;

    private void Start()
    {
        GetReferences();
    }

    private void Update()
    {
        Vault();
    }

    private void Vault()
    {
        RaycastHit hit;
        _canVault = Physics.Raycast(transform.position, _playerModel.forward, out hit, 0.5f, _vaultLayer);
        Debug.DrawLine(transform.position, transform.position + _playerModel.forward * 0.5f, Color.blue);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(_canVault)
            {
                Debug.Log("Vaulting");

                _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
                _rb.AddForce(transform.up * 20f, ForceMode.Force);
            }
        }
    }

    private void GetReferences()
    {
        _rb = GetComponent<Rigidbody>();
    }
}
