using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButtonScript : MonoBehaviour
{
    public Transform button;
    public GameObject doorP;
    float smooth = 10f;
    bool isPressed = false;
    Quaternion start = Quaternion.Euler(0f, 0f, 0f);
    Quaternion target = Quaternion.Euler(0f, 180f, 0f);

    private void FixedUpdate()
    {
        isPressed = false;
        if (!isPressed) 
        {
            doorP.transform.rotation = Quaternion.Slerp(doorP.transform.rotation, start, Time.deltaTime * smooth);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "pickable")
        {
            isPressed = true;
            doorP.transform.rotation = Quaternion.Slerp(doorP.transform.rotation, target, Time.deltaTime*smooth);
            //Debug.Log("dziala");
        }
    }
}