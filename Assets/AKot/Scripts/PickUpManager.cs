using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    [SerializeField] private bool isPickedUp = false;
    [SerializeField] private string pickableTag = "pickable";
    [SerializeField] private Material defaultMaterial, highlithedMaterial;
    [SerializeField] private float maxPickUpDistance = 5f;
    private Transform _selection, player;
    private PickUp pickScript;

    private void Start()
    {
        player = Camera.main.transform;
    }
    private void Update()
    {
        if (_selection != null && _selection != pickScript?.transform)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            _selection = null;
        }

        if (isPickedUp)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isPickedUp = false;
                if (pickScript != null)
                {
                    pickScript.Drop();
                    pickScript = null;
                }
            }
            return;
        }


        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;
            if (selection.CompareTag(pickableTag))
            {
                float distanceToPlayer = Vector3.Distance(player.position, selection.position);
                if (distanceToPlayer <= maxPickUpDistance)
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    if (selectionRenderer != null)
                    {
                        selectionRenderer.material = highlithedMaterial;
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            if (isPickedUp != true)
                            {
                                isPickedUp = true;
                                pickScript = selection.GetComponent<PickUp>();
                                pickScript.Pick();
                            }
                        }
                    }
                    _selection = selection;
                }
            }
        }
    }
}
