using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverDissCondition : MonoBehaviour
{
    int leversActivated, requiredLevers;

    public GameObject doorP;

    // Start is called before the first frame update
    void Start()
    {
        leversActivated = 0;
        requiredLevers = GameObject.FindGameObjectsWithTag("Lever").Length;
        Debug.Log("Requaired: " + requiredLevers);
    }

    public void LeverActivated()
    {
        leversActivated++;
        Debug.Log("Activated: " + leversActivated);
        if (leversActivated >= requiredLevers)
        {
            Debug.Log("Open");
            doorP.SetActive(false);
        }
    }
}
