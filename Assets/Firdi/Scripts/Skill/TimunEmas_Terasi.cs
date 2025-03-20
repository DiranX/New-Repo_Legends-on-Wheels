using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimunEmas_Terasi : MonoBehaviour
{
    Rigidbody rb;
    bool isContact;
    public GameObject Terasi;
    public GameObject Lumpur;
    Collider Collider;
    public bool isTerasi;
    public bool isLumpur;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Collider = GetComponent<BoxCollider>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isTerasi)
        {
            if (collision.gameObject.CompareTag("Track"))
            {
                if (!isContact)
                {
                    Terasi.SetActive(false);
                    Lumpur.SetActive(true);
                    isContact = true;
                    rb.mass = 100;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.isKinematic = true;
                    Collider.isTrigger = true;
                    Physics.IgnoreLayerCollision(12, 11, false);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isLumpur)
        {
            if (other.gameObject.CompareTag("Kart"))
            {
                Debug.Log("Buta");
                other.GetComponent<Skill_Effect>().lumpur.SetActive(true);
            }
        }
    }
}
