using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class Pitung_Golok : MonoBehaviour
{
    public int Id;
    public bool isBack;
    public Pitung_Skill pitung;
    float speed = 10;
    public float timer;

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (timer >= 2) isBack = true;

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (isBack)
        {
            Vector3 target = pitung.GetComponent<Transform>().position;
            Vector3 newPos = Vector3.MoveTowards(rigidbody.position, target, .5f);
            rigidbody.MovePosition(newPos);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerKartController playerKart = other.GetComponent<PlayerKartController>();
        Pitung_Skill id = other.GetComponent<Pitung_Skill>();

        if(id != null)
        {
            if(playerKart != null)
            {
                if (isBack)
                {
                    if (other.gameObject.CompareTag("Kart") && Id == id.Id)
                    {
                        Destroy(gameObject);
                    }
                    else if (other.gameObject.CompareTag("Kart") && Id != id.Id)
                    {
                        other.gameObject.GetComponent<Animator>().SetTrigger("Stop");
                    }
                }
            }
            
        }
    }
}
