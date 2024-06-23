using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private string pressableTag = "pressable";
    private Transform _selection;
    public Transform player;
    public float buttonRange = 4f;
    public float timerTime = 5f, timerBackup = 5f;
    private bool isPressed = false;
    public GameObject doorP;
    private float smooth = 10f;
    private Quaternion start = Quaternion.Euler(0f, 115f, 0f);
    private Quaternion target = Quaternion.Euler(0f, 0f, 0f);

    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var selection = hit.transform;
            if (selection.CompareTag(pressableTag))
            {
                float distanceToPlayer = Vector3.Distance(player.position, selection.position);
                if (distanceToPlayer <= buttonRange)
                {
                    _selection = selection;
                    if (Input.GetKeyDown(KeyCode.E) && !isPressed)
                    {
                        Debug.Log("E zosta³o nacisniete");
                        isPressed = true;
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
                timerTime = timerBackup;
            }

            if (isPressed)
            {
                doorP.transform.rotation = Quaternion.Slerp(doorP.transform.rotation, target, Time.deltaTime * smooth);
            }
            else
            {
                doorP.transform.rotation = Quaternion.Slerp(doorP.transform.rotation, start, Time.deltaTime * smooth);
            }
        }
        else
        {
            if (doorP.transform.rotation != start)
            {
                doorP.transform.rotation = Quaternion.Slerp(doorP.transform.rotation, start, Time.deltaTime * smooth);
            }
        }
    }
}
