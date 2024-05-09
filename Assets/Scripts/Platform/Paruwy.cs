using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paruwy : MonoBehaviour
{
  // Update is called once per frame
   
         // Si�a odpychania
        public float force = 10f;

        void OnCollisionEnter(Collision collision)
        {
            // Sprawd�, czy kolizja nast�pi�a z obiektem, kt�ry jest oznaczony jako gracz
            if (collision.gameObject.CompareTag("Player"))
            {
                // Oblicz kierunek odpychania
                Vector3 pushDirection = (collision.transform.position - transform.position).normalized;

                // Dodaj si�� odpychania do obiektu, kt�ry uderzy� w nasz obiekt
                collision.rigidbody.AddForce(pushDirection * force, ForceMode.Impulse);
            }
        }
    
}

