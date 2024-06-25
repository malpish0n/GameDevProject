using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class TargetBudda : MonoBehaviour
{
    int targetsDestroyed, requiredTargets;
    public GameObject particle;

    // Start is called before the first frame update
    void Start()
    {
        targetsDestroyed = 0;
        requiredTargets = GameObject.FindGameObjectsWithTag("Target").Length;
        Debug.Log("Requaired: " + requiredTargets);
        particle.SetActive(false);
    }

    public void TargetDestroyed()
    {
        targetsDestroyed++;
        Debug.Log("Destoyered: " + targetsDestroyed);
        if (targetsDestroyed >= requiredTargets)
        {
            Debug.Log("Budda Happy");
            particle.SetActive(true);
        }
    }
}
