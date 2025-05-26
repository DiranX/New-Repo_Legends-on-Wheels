using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item_Barrel_Script : MonoBehaviour
{
    Rigidbody rb;
    bool isContact;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.IgnoreLayerCollision(11, 3, true);
    }
    private void Update()
    {
        StartCoroutine(Dest());
    }
    IEnumerator Dest()
    {
        yield return new WaitForSeconds(10f);
        if (!isContact)
        {
            Destroy(this.gameObject);
        }
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
                transform.localScale *= 3f;
                Physics.IgnoreLayerCollision(11, 3, false);
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            BoxCollider collider = GetComponent<BoxCollider>();
            if (collision.gameObject.GetComponent<PlayerItemHolder>().playerKartController.GetComponent<Skill_Effect>().isProtect != true)
            {
                collision.gameObject.GetComponent<PlayerItemHolder>().playerKartController.GetComponent<Animator>().SetTrigger("Stop");
                collision.gameObject.GetComponent<PlayerItemHolder>().Sfx.PlayOneShot(
                    collision.gameObject.GetComponent<PlayerItemHolder>().SfxSound[0]);
                Destroy(this.gameObject);
            }
            else
            {
                collision.gameObject.GetComponent<PlayerItemHolder>().Sfx.PlayOneShot(
                    collision.gameObject.GetComponent<PlayerItemHolder>().SfxSound[0]);
                Destroy(this.gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject);
        }
    }
}
