using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerWall : MonoBehaviour
{
    [SerializeField] private string pressableTag = "pressable";
    private Transform _selection;
    public Transform player;
    public float buttonRange = 4f;
    public float timerTime = 5f, timerBackup = 5f;
    private bool isPressed = false;
    public GameObject wall;
    public GameObject wallTwo;
    public GameObject wallThree;
    public GameObject wallFour;
    public GameObject particle;

    private void Start()
    {
        particle.SetActive(false);
        wall.SetActive(false);
        wallTwo.SetActive(false);
    }

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
                particle.SetActive(true);
                wall.SetActive(true);
                wallTwo.SetActive(true);
                wallThree.SetActive(true);
                wallFour.SetActive(true);
            }
            else
            {
                particle.SetActive(false);
                wall.SetActive(false);
                wallTwo.SetActive(false);
                wallThree.SetActive(false);
                wallFour.SetActive(false);
            }
        }
    }
}
