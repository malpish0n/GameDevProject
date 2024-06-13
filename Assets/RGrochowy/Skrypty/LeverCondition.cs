using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverCondition : MonoBehaviour
{
    int leversActivated, requiredLevers;

    public GameObject doorP;
    float smooth = 10f;
    Quaternion target = Quaternion.Euler(0f, 180f, 0f);
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
            doorP.transform.rotation = Quaternion.Lerp(doorP.transform.rotation, target, Time.deltaTime * smooth);
        }
    }
}
