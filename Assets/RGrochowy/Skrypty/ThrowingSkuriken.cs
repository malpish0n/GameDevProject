using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThrowingSkuriken : MonoBehaviour
{
    public GameObject Player;

    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;

    public TMP_Text ShurikenUINumber;
    public GameObject ShurikenUI;

    public int totalThrows=10;
    public float throwCooldown=0.5f;

    public KeyCode throwKey = KeyCode.Mouse0;
    public float throwForce=35;
    public float throwUpwardForce;

    bool readyToThrow;

    private void Start()
    {
        readyToThrow = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrows>0)
        {
            Throw();
        }
    }

    private void Throw()
    {
        readyToThrow = false;
       

        //instantiate object to throw
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        //get rigidbody component
        Rigidbody projectileRb=projectile.GetComponent<Rigidbody>();

        //calculate direction
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if(Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        //add force
        Vector3 forceToAdd =forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;
        ShurikenUINumber.text = totalThrows.ToString();

        //impement throwCooldown
        Invoke(nameof(ResetThrow), throwCooldown);

        if (totalThrows <= 0)
        {
            ShurikenUI.SetActive(false);
            Player.GetComponent<ThrowingSkuriken>().enabled = false;
        }   
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }
}
