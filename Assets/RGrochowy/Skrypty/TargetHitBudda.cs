using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHitBudda : MonoBehaviour
{
    public TargetBudda TC;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            TC.TargetDestroyed();
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
