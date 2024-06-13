using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Transform player;
    [SerializeField] public float buttonRange = 4;
    private bool isPressed = false;
    public Material material1, material2;
    public LeverCondition LC;

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 distanceToPlayer = transform.position - player.position;
        if (distanceToPlayer.magnitude <= buttonRange)
        {
            if (Input.GetKeyDown(KeyCode.E)&& isPressed != true)
            {
                isPressed = true;
                GetComponent<MeshRenderer>().material = material2;
                LC.LeverActivated();
            }
        }
    }
}
