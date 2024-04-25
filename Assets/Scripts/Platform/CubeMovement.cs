using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = transform; // Ustawia platformê jako rodzica gracza
            Debug.Log("Kolizja");
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = null; // Resetuje rodzica gracza, gdy opuszcza platformê
        }
    }

}
