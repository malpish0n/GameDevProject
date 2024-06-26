using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paruweczki : MonoBehaviour
{
    // Si³a, z jak¹ gracz zostanie wyrzucony
    public float launchForce = 1000f;
    // Kierunek, w którym gracz zostanie wyrzucony
    public Vector3 launchDirection = Vector3.up;

    private void OnCollisionEnter(Collision collision)
    {
        // SprawdŸ, czy obiekt koliduj¹cy ma tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Pobierz komponent Rigidbody z obiektu gracza
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            if (playerRigidbody != null)
            {
                // Wyczyœæ wszelkie istniej¹ce si³y dzia³aj¹ce na gracza
                playerRigidbody.velocity = Vector3.zero;

                // Dodaj si³ê wyrzucaj¹c¹ gracza
                playerRigidbody.AddForce(launchDirection.normalized * launchForce);
            }
        }
    }
}