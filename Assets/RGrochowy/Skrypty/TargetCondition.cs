using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class TargetCondition : MonoBehaviour
{
    int targetsDestroyed, requiredTargets;

    public GameObject doorP;

    // Start is called before the first frame update
    void Start()
    {
        targetsDestroyed = 0;
        requiredTargets = GameObject.FindGameObjectsWithTag("Target").Length;
        Debug.Log("Requaired: " + requiredTargets);
    }

    public void TargetDestroyed()
    {
        targetsDestroyed++;
        Debug.Log("Destoyered: " + targetsDestroyed);
        if (targetsDestroyed >= requiredTargets)
        {
            Debug.Log("Open");
            doorP.SetActive(false);
        }
    }
}
