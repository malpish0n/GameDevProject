using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform objectHolder, cam;
    public float dropForwardForce=2f, dropUpwardForce=2f;
    private FloorTriggerScript floorTriggerScript;

    private void Start()
    {
        floorTriggerScript = FindObjectOfType<FloorTriggerScript>();
    }

    public void Pick()
    {
        if (floorTriggerScript != null)
        {
            floorTriggerScript.HandleManualExit(GetComponent<Collider>());
        }

        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().freezeRotation = true;
        GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        GetComponent<Rigidbody>().isKinematic = true;
        this.transform.position = objectHolder.position;
        this.transform.parent = objectHolder.transform;
    }

    public void Drop()
    {
        this.transform.parent = null;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<Rigidbody>().freezeRotation = false;
        GetComponent<Rigidbody>().isKinematic = false;

        Vector3 forceDirection = cam.transform.forward;
        GetComponent<Rigidbody>().AddForce(cam.forward * dropForwardForce, ForceMode.Impulse);
        GetComponent<Rigidbody>().AddForce(cam.up * dropUpwardForce, ForceMode.Impulse); 
    }
}
