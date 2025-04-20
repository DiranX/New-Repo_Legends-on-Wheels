using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sangkuriang_Bukit : MonoBehaviour
{
    public int Id;
    public float boostAmount = 30f; // Adjust boost power
    public float boostDuration = 2f; // Adjust boost time

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
            Sangkuriang_Skill Sangkuriang = other.GetComponent<Sangkuriang_Skill>();

            if(Sangkuriang != null)
            {
                if(Sangkuriang.Id == Id)
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
            }
        }

    }
}
