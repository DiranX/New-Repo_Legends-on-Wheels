using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Bomb_Script : MonoBehaviour
{
    public GameObject bomb;
    public GameObject bombVfx;
    public float time;
    public bool isContact;
    public bool isVfx;
    public bool isExplode;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!isVfx)
        {
            Physics.IgnoreLayerCollision(11, 3, true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isVfx)
        {
            if (!isContact)
            {
                time = 0;
            }
            else
            {
                time += Time.deltaTime;

                if (time >= 1 && !isExplode)
                {
                    isExplode = true;
                    bombVfx.SetActive(true);
                    bomb.SetActive(false);
                }
            }
        }

        if (time > 3)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isVfx)
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
                }
            }

            if (collision.gameObject.CompareTag("Player"))
            {
                if (isContact && !isExplode)
                {
                    PlayerKartController playerKart = collision.gameObject.GetComponent<PlayerItemHolder>().playerKartController;
                    if (playerKart != null)
                    {
                        isExplode = true;
                        bombVfx.SetActive(true);
                        bomb.SetActive(false);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isVfx)
        {
            if (other.gameObject.CompareTag("Kart"))
            {
                PlayerKartController playerKart = other.GetComponent<PlayerKartController>();
                if (playerKart != null)
                {
                    if (playerKart.GetComponent<Skill_Effect>().isProtect != true)
                    {
                        other.gameObject.GetComponent<Animator>().SetTrigger("Stop");
                        Debug.Log("Stop");
                    }
                }
            }
        }
    }
}
