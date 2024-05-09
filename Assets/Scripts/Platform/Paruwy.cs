using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paruwy : MonoBehaviour
{
  // Update is called once per frame
   
         // Si³a odpychania
        public float force = 10f;

        void OnCollisionEnter(Collision collision)
        {
            // SprawdŸ, czy kolizja nast¹pi³a z obiektem, który jest oznaczony jako gracz
            if (collision.gameObject.CompareTag("Player"))
            {
                // Oblicz kierunek odpychania
                Vector3 pushDirection = (collision.transform.position - transform.position).normalized;

                // Dodaj si³ê odpychania do obiektu, który uderzy³ w nasz obiekt
                collision.rigidbody.AddForce(pushDirection * force, ForceMode.Impulse);
            }
        }
    
}

