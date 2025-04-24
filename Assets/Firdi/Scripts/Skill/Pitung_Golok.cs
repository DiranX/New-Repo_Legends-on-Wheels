using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pitung_Golok : MonoBehaviour
{
    public int Id;
    public Vector3 Thrower;
    public bool isBack;
    private void Update()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(ComeBack());
        if (isBack)
        {
            Vector3 target = new Vector3(Thrower.x, Thrower.y + .1f + Thrower.z);
            Vector3 newPos = Vector3.MoveTowards(rigidbody.position, target, 1);
            rigidbody.MovePosition(newPos);
        }
    }
    IEnumerator ComeBack()
    {
        yield return new WaitForSeconds(1);
        isBack = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerKartController playerKart = other.GetComponent<PlayerKartController>();
        Pitung_Skill id = other.GetComponent<Pitung_Skill>();

        if(id != null)
        {
            if(playerKart != null)
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
