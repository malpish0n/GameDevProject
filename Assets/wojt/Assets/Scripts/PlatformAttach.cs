using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttach : MonoBehaviour
{
    private Transform playerTransform;
    private Vector3 lastPlatformPosition;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerTransform = collision.transform;
            lastPlatformPosition = transform.position;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerTransform = null;
        }
    }

    private void FixedUpdate()
    {
        if (playerTransform != null)
        {
            Vector3 deltaPosition = transform.position - lastPlatformPosition;
            playerTransform.position += deltaPosition;
        }
        lastPlatformPosition = transform.position;
    }
}
