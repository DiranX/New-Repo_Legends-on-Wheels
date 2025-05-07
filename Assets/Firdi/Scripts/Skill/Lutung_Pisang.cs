using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lutung_Pisang : MonoBehaviour
{
    Rigidbody rb;
    bool isContact;
    BoxCollider collider;
    public bool isPisangLempar;
    public bool isPisang;
    public int Id;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
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
                collider.isTrigger = true;
                Physics.IgnoreLayerCollision(12, 11, false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Kart"))
        {
            PlayerKartController playerKartController = other.GetComponent<PlayerKartController>();
            int id = playerKartController.GetComponentInParent<Player>().id;

            if (this.Id != id)
            {
                if (playerKartController != null)
                {
                    other.GetComponent<Animator>().SetTrigger("Stop");
                    Destroy(gameObject);
                }
            }
        }
    }
}
