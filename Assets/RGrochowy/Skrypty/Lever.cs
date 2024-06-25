using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private string leverTag = "Lever";
    public Transform player;
    public float buttonRange = 4f;
    private bool isPressed = false;
    public Material material2;
    public LeverCondition LC;

    // Update is called once per frame
    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var selection = hit.transform;
            if (selection.CompareTag(leverTag) && selection==transform)
            {
                float distanceToPlayer = Vector3.Distance(player.position, selection.position);
                if (distanceToPlayer <= buttonRange)
                {
                    if (Input.GetKeyDown(KeyCode.E) && !isPressed)
                    {
                        isPressed = true;
                        GetComponent<MeshRenderer>().material = material2;
                        LC.LeverActivated();
                    }
                }
            }
        }
    }
}
