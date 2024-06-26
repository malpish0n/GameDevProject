using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paruweczki : MonoBehaviour
{
    // Si�a, z jak� gracz zostanie wyrzucony
    public float launchForce = 1000f;
    // Kierunek, w kt�rym gracz zostanie wyrzucony
    public Vector3 launchDirection = Vector3.up;

    private void OnCollisionEnter(Collision collision)
    {
        // Sprawd�, czy obiekt koliduj�cy ma tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Pobierz komponent Rigidbody z obiektu gracza
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            if (playerRigidbody != null)
            {
                // Wyczy�� wszelkie istniej�ce si�y dzia�aj�ce na gracza
                playerRigidbody.velocity = Vector3.zero;

                // Dodaj si�� wyrzucaj�c� gracza
                playerRigidbody.AddForce(launchDirection.normalized * launchForce);
            }
        }
    }
}