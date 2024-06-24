using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointPosistion : MonoBehaviour
{
    [SerializeField] private GameObject checkpointReturnTrigger;
    [SerializeField] private ReturnToCheckpoint returnTo;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerCollider")
        {
            returnTo.checkpointPos=transform.position;
        }
    }
}
