using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BosstPad : MonoBehaviour
{
    public float boostAmount = 30f; // Adjust boost power
    public float boostDuration = 2f; // Adjust boost time

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Kart"))
        {
            // Check if the player kart enters the boost pad
            PlayerKartController playerKart = other.GetComponent<PlayerKartController>();

            //playerKart.boostPad = true;

            if (playerKart != null)
            {
                if (playerKart.moveForward || playerKart.moveBackward)
                {
                    playerKart.ReceiveBoost(boostAmount, boostDuration);
                    playerKart.PlayBoostParticle();
                }
                else if (!playerKart.moveForward || !playerKart.moveBackward)
                {
                    playerKart.ReceiveBoost(boostAmount * 2, boostDuration / 2);
                    playerKart.PlayBoostParticle();
                }
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Kart"))
        {
            // Check if the player kart enters the boost pad
            PlayerKartController playerKart = other.GetComponent<PlayerKartController>();

            //playerKart.boostPad = false;
        }
    }
}


