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
    BoxCollider collider;
    public bool isTerasi;
    public bool isLumpur;
    public int Id;
    AudioSource audioSource;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Track"))
        {
            if (!isContact)
            {
                audioSource.Play();
                Terasi.SetActive(false);
                Lumpur.SetActive(true);
                isContact = true;
                rb.mass = 100;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
                collider.isTrigger = true;
                collider.size = new Vector3(3.5f, 0.5f, 3.5f);
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

            if (this.Id != id && playerKartController.GetComponent<Skill_Effect>().isProtect != true)
            {
                if (playerKartController != null)
                {
                    Debug.Log("Buta");
                    other.GetComponent<Skill_Effect>().lumpur.SetActive(true);
                    playerKartController.sphere.GetComponent<PlayerItemHolder>().Sfx.PlayOneShot(
                        playerKartController.sphere.GetComponent<PlayerItemHolder>().SfxSound[3]);
                    Destroy(gameObject);
                }
            }
        }
    }
}
