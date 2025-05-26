using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitung_SkillTrigger : MonoBehaviour
{
    public int Id;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Kart"))
        {
            // Check if the player kart enters the boost pad
            PlayerKartController playerKart = other.gameObject.GetComponent<PlayerKartController>();
            int id = playerKart.GetComponentInParent<Player>().id;

            if (this.Id != id && playerKart.GetComponent<Skill_Effect>().isProtect == false)
            {
                playerKart.GetComponent<Animator>().SetTrigger("Stop");
                playerKart.GetComponent<Skill_Effect>().isReverse = true;
                Debug.Log("Stop");
                Debug.Log("Golok Id =" + this.Id + "Kart Id =" + id);
            }
        }
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Destroy(other.gameObject);
        }
    }
}
