using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToCheckpoint : MonoBehaviour
{
    public Vector3 checkpointPos;
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
            GameObject player = GameObject.Find("Player");
            player.transform.position = checkpointPos;
        }
    }
}
