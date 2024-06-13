using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private string pressableTag = "pressable";
    [SerializeField] private Material defaultMaterial, highlithedMaterial, pressedMaterial;
    private Transform _selection;
    public Transform player;
    public float buttonRange = 4f;
    public float timerTime = 5f, timerBackup = 5f;
    private bool isPressed = false;
    public GameObject doorP;
    private float smooth = 10f;
    private Quaternion start = Quaternion.Euler(0f, 0f, 0f);
    private Quaternion target = Quaternion.Euler(0f, 180f, 0f);
    private Renderer _selectionRenderer;

    private void Update()
    {
        if (_selection != null && !_selection.CompareTag(pressableTag))
        {
            _selectionRenderer.material = defaultMaterial;
            _selection = null;
            _selectionRenderer = null;
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var selection = hit.transform;
            if (selection.CompareTag(pressableTag))
            {
                float distanceToPlayer = Vector3.Distance(player.position, selection.position);
                if (distanceToPlayer <= buttonRange)
                {
                    _selectionRenderer = selection.GetComponent<Renderer>();
                    _selectionRenderer.material = highlithedMaterial;
                    if (Input.GetKeyDown(KeyCode.E) && !isPressed)
                    {
                        Debug.Log("E zosta³o nacisniete");
                        isPressed = true;
                        DoorOpen();
                        _selectionRenderer.material = pressedMaterial;
                    }
                }
            }
        }

        if (isPressed)
        {
            

            timerTime -= Time.deltaTime;
            Debug.Log("Zosta³o: " + timerTime);

            if (timerTime <= 0)
            {
                isPressed = false;
                _selectionRenderer.material = defaultMaterial;
                timerTime = timerBackup;
            }

            if (!isPressed)
            {
                doorP.transform.rotation = Quaternion.Slerp(doorP.transform.rotation, start, Time.deltaTime * smooth);
            }
        }
    }

    private void DoorOpen()
    {
        doorP.transform.rotation = Quaternion.Slerp(doorP.transform.rotation, target, Time.deltaTime * smooth);
    }
}
