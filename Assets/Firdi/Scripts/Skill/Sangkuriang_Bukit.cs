using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sangkuriang_Bukit : MonoBehaviour
{
    public int Id;
    public float boostAmount = 30f; // Adjust boost power
    public float boostDuration = 2f; // Adjust boost time

    void Start()
    {
        AlignToGround();
    }

    void AlignToGround()
    {
        RaycastHit hit;
        // Cast a ray downward from a bit above the object's position
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 10f))
        {
            // Align the up vector of the object to match the normal of the ground
            Quaternion groundRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            transform.rotation = groundRotation * Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }

    private void Update()
    {
        if (this.gameObject.activeSelf)
        {
            StartCoroutine(Timeup());
        }
    }

    IEnumerator Timeup()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Kart"))
        {
            // Check if the player kart enters the boost pad
            PlayerKartController playerKart = other.GetComponent<PlayerKartController>();
            int id = playerKart.GetComponentInParent<Player>().id;

            if (id == Id)
            {
                if (playerKart != null)
                {
                    if (playerKart.moveForward || playerKart.moveBackward)
                    {
                        playerKart.ReceiveBoost(boostAmount, boostDuration);
                        playerKart.PlayBoostParticle();
                    }
                    else if (!playerKart.moveForward || !playerKart.moveBackward)
                    {
                        playerKart.ReceiveBoost(boostAmount * 1.5f, boostDuration);
                        playerKart.PlayBoostParticle();
                    }
                }
            }
            else
            {
                playerKart.GetComponent<Skill_Effect>().isSlowed = true;
                playerKart.topSpeed = playerKart.topSpeed / 2;
            }
        }

    }
}
