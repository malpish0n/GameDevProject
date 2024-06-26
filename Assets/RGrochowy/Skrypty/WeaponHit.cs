using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WeaponHit : MonoBehaviour
{

    public void OnCollisionEnter(Collision collision)
    {


        if (collision.gameObject.name == "Player")
        {
            GameObject BlackScreen = GameObject.Find("FadeToBlack");
            GameObject CheckpointReturn = GameObject.Find("ReturnToCheckpoint");
            GameObject player = GameObject.Find("Player");
            if (CheckpointReturn != null)
            {
                FadeEffect fadeScript=BlackScreen.GetComponent<FadeEffect>();
                ReturnToCheckpoint script = CheckpointReturn.GetComponent<ReturnToCheckpoint>();
                PlayerMovement movement = player.GetComponent<PlayerMovement>();
                ThrowingSkuriken shurikens = player.GetComponent<ThrowingSkuriken>();
                if (script != null)
                {
                    movement.enabled = false;
                    shurikens.DisableShurikens();
                    StartCoroutine(fadeScript.FadeToBlackAndBack(script.Return));
                }

            }
        }
    }
}
