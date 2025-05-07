using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoroJongrang_Candi : MonoBehaviour
{
    [SerializeField]Rigidbody rb;
    bool isContact;
    public int id;

    void Start()
    {
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
                transform.localScale *= 3f;
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerKartController playerkart = collision.gameObject.GetComponent<PlayerItemHolder>().playerKartController;
            int id = playerkart.GetComponentInParent<Player>().id;
            if(this.id != id && playerkart.GetComponent<Skill_Effect>().isProtect != true)
            {
                collision.gameObject.GetComponent<PlayerItemHolder>().playerKartController.GetComponent<Animator>().SetTrigger("Stop");
                Destroy(this.gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject);
        }
    }
}
