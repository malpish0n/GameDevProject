using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTargets : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        foreach (GameObject obj in targets)
        {
            MeshRenderer mesh=obj.GetComponent<MeshRenderer>();
            mesh.enabled = true;
        }
    }
}
