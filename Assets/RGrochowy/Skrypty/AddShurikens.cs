using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class AddShurikens : MonoBehaviour
{
    public GameObject Player;
    public GameObject ShurikenUI;
    public TMP_Text ShurikenUINumber;
    public ThrowingSkuriken Shurikens;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerCollider")
        {
            Player.GetComponent<ThrowingSkuriken>().enabled = true;
            Shurikens.totalThrows = 10;
            ShurikenUINumber.text = "10";
            ShurikenUI.SetActive(true);
        }
    }
}