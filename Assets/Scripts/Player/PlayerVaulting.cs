using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVaulting : MonoBehaviour
{
    [SerializeField] private LayerMask _vaultLayer;
    [SerializeField] private Transform _player;
    private Rigidbody _rb;
    private bool _canVault;
    private bool _isVaulting;

    [SerializeField] private int raycastCount;
    [SerializeField] private float raycastGap;
    [SerializeField] private float raycastDistance;
    [SerializeField] private bool show_debug_line;
    [SerializeField] private float vaultSpeed;

    private List<RaycastData> cast_list = new List<RaycastData>();
    private float vaultTargetHeight; // Store the height of the obstacle

    private void Start()
    {
        GetReferences();
        GenerateRaycast();
    }

    private void Update()
    {
        CheckRaycasts();

        if (_canVault && Input.GetKeyDown(KeyCode.Space) && !_isVaulting)
        {
            StartCoroutine(Vault());
        }
    }

    private void GenerateRaycast()
    {
        for (int i = 0; i < raycastCount; i++)
        {
            GameObject raycast = new GameObject("Raycast" + i);
            raycast.transform.parent = transform;
            raycast.transform.localPosition = new Vector3(0, i * raycastGap, 0);

            RaycastData raycastData = new RaycastData
            {
                raycastObject = raycast,
                showDebug = show_debug_line
            };
            cast_list.Add(raycastData);
        }
    }

    private void GetReferences()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void CheckRaycasts()
    {
        _canVault = false;
        foreach (var cast in cast_list)
        {
            Vector3 rayOrigin = cast.raycastObject.transform.position;
            Ray ray = new Ray(rayOrigin, transform.forward);
            if (cast.showDebug)
            {
                Debug.DrawRay(rayOrigin, transform.forward * raycastDistance, Color.red);
            }

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, raycastDistance, _vaultLayer))
            {
                // Log heights for debugging
                Debug.Log("Hit point y: " + hit.point.y);
                Debug.Log("Player position y: " + transform.position.y);

                // Calculate vaultTargetHeight relative to player's position
                vaultTargetHeight = hit.point.y - transform.position.y;
                _canVault = true;
                Debug.Log("Vault possible! Height: " + vaultTargetHeight);
                break;
            }
        }
    }


    private IEnumerator Vault()
    {
        _isVaulting = true;
        _rb.useGravity = false;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + vaultTargetHeight, startPosition.z);

        float elapsedTime = 0f;
        while (elapsedTime < vaultSpeed)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / vaultSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;

        _rb.useGravity = true;
        _isVaulting = false;
    }

    private class RaycastData
    {
        public GameObject raycastObject;
        public bool showDebug;
    }
}
