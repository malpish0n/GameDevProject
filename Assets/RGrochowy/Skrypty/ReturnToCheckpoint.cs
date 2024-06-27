using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToCheckpoint : MonoBehaviour
{
    public Vector3 checkpointPos;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Return();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerCollider")
        {
            GameObject player = GameObject.Find("Player");
            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            ThrowingSkuriken shurikens = player.GetComponent<ThrowingSkuriken>();
            GameObject BlackScreen = GameObject.Find("FadeToBlack");
            FadeEffect fadeScript = BlackScreen.GetComponent<FadeEffect>();
            movement.enabled = false;
            shurikens.DisableShurikens();
            StartCoroutine(fadeScript.FadeToBlackAndBack(Return));
        }
    }

    public void Return()
    {
        GameObject player = GameObject.Find("Player");
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        movement.enabled = true;
        player.transform.position = checkpointPos;
    }
}
