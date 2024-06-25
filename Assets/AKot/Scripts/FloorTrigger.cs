using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTriggerScript : MonoBehaviour
{
    public GameObject particle;

    private void Start()
    {
        particle.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("pickable"))
        {
            particle.SetActive(true);
            Debug.Log("Pickable object entered the trigger. Particle Enabled.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "pickable")
        {
            particle.SetActive(false);
            Debug.Log("Trigger Exit: Particle Disabled");
        }
    }

    public void HandleManualExit(Collider other)
    {
        if (other.gameObject.tag == "pickable")
        {
            particle.SetActive(false);
            Debug.Log("Manual Trigger Exit: Particle Disabled");
        }
    }
}
