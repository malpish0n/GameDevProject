using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string sceneName;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "PlayerCollider")
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
