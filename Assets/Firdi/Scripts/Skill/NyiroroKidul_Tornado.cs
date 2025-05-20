using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
                other.gameObject.GetComponent<PlayerItemHolder>().playerKartController.GetComponent<Animator>().SetTrigger("Spin");
                other.transform.DOMove
                    (other.gameObject.GetComponent<PlayerItemHolder>().playerKartController.GetComponent<Skill_Effect>()
                    .roroKidulRelocate.transform.position, 50 * Time.deltaTime);
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
