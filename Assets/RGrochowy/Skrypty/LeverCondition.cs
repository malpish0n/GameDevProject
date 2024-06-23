using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverCondition : MonoBehaviour
{
    int leversActivated, requiredLevers;

    public GameObject doorP;
    private float smooth = 10f;
    private Quaternion start = Quaternion.Euler(0f, 115f, 0f);
    private Quaternion target = Quaternion.Euler(0f, 0f, 0f);
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
            doorP.transform.rotation = Quaternion.Slerp(doorP.transform.rotation, target, Time.deltaTime * smooth);
        }
        else
        {
            doorP.transform.rotation = Quaternion.Slerp(doorP.transform.rotation, start, Time.deltaTime * smooth);
        }
        
        if (doorP.transform.rotation != start)
        {
            doorP.transform.rotation = Quaternion.Slerp(doorP.transform.rotation, start, Time.deltaTime* smooth);
        }
    }
}
