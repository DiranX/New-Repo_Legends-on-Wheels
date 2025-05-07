using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NyiroroKidul_Tornado : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(other.gameObject.GetComponent<PlayerItemHolder>().playerKartController.GetComponent<Skill_Effect>().isProtect != true)
            {
                other.gameObject.GetComponent<PlayerItemHolder>().playerKartController.GetComponent<Animator>().SetTrigger("Stop");
                Debug.Log("Tornado Hit Player");
            }
        }

        if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit Obstacle");
            Destroy(other.gameObject);
        }
    }
}
