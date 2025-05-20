using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Relocate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Respawner"))
        {
            transform.DOMove(other.GetComponent<Respawn>().ReLocate.transform.position, 50 * Time.deltaTime);
            GetComponent<PlayerItemHolder>().playerKartController.currentSpeed = 0;
        }
    }
}
