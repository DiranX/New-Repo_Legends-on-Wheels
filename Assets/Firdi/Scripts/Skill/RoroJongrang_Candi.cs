using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoroJongrang_Candi : MonoBehaviour
{
    [SerializeField]Rigidbody rb;
    bool isContact;
    public bool isClose;
    public int id;

    void Start()
    {
        Physics.IgnoreLayerCollision(12, 11, true);
    }

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Kart");
        if (Vector3.Distance(this.gameObject.transform.position, player.transform.position) <= 15)
        {
            if(player.GetComponent<PlayerKartController>().ID == id)
            {
                GetComponent<BoxCollider>().isTrigger = true;
            }
        }
        else if(Vector3.Distance(this.gameObject.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 5)
        {
            GetComponent<BoxCollider>().isTrigger = false;
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
            //else if (this.id == id && isClose)
            //{
            //    GetComponent<BoxCollider>().isTrigger = true;
                
            //}
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerKartController playerkart = other.gameObject.GetComponent<PlayerItemHolder>().playerKartController;
            int id = playerkart.GetComponentInParent<Player>().id;

            if (this.id == id)
            {
                playerkart.ReceiveBoost(1, 1);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerKartController playerkart = other.gameObject.GetComponent<PlayerItemHolder>().playerKartController;
            int id = playerkart.GetComponentInParent<Player>().id;

            if (this.id == id)
            {
                GetComponent<BoxCollider>().isTrigger = false;
            }
        }
    }
}
