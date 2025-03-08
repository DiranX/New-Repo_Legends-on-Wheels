using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Barrel_Script : MonoBehaviour
{
    Rigidbody rb;
    bool isContact;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.IgnoreLayerCollision(12, 11, true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Track"))
        {
            if (!isContact)
            {
                isContact = true;
                rb.mass = 100;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
                Physics.IgnoreLayerCollision(12, 11, false);
                transform.localScale *= 12f;
            }
        }
    }
}
