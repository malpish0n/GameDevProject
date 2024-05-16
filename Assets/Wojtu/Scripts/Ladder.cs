using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    // Start is called before the first frame update
    public class ClimbLadder3D : MonoBehaviour
    {
        public float climbSpeed = 3f;
        private bool isClimbing = false;
        private Rigidbody rb;
        private Collider ladderCollider;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (isClimbing && ladderCollider != null)
            {
                float verticalInput = Input.GetAxis("Vertical");
                rb.velocity = new Vector3(rb.velocity.x, verticalInput * climbSpeed, rb.velocity.z);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ladder"))
            {
                ladderCollider = other;
                isClimbing = true;
                rb.useGravity = false; // Disable gravity when climbing
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Ladder"))
            {
                ladderCollider = null;
                isClimbing = false;
                rb.useGravity = true; // Re-enable gravity when not climbing
            }
        }
    }


