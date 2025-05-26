using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButoIjo_Shock : MonoBehaviour
{
    public int Id;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if(timer > 2)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Kart"))
        {
            PlayerKartController playerKart = other.GetComponent<PlayerKartController>();
            int id = playerKart.GetComponentInParent<Player>().id;
            if (playerKart != null)
            {
                if(this.Id != id&& playerKart.GetComponent<Skill_Effect>().isProtect != true)
                {
                    playerKart.GetComponent<Skill_Effect>().isSlowed = true;
                    playerKart.topSpeed = playerKart.topSpeed / 2;
                    playerKart.GetComponent<Animator>().SetTrigger("Stop");
                    Debug.Log("Stop");
                }
            }
        }
    }
}
