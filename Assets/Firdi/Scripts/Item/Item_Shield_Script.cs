using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Shield_Script : MonoBehaviour
{
    float time;

    private void Update()
    {
        time += Time.deltaTime;

        if(time >= 10)
        {
            this.gameObject.SetActive(false);
            time = 0;
            GetComponentInParent<PlayerItemHolder>().playerKartController.GetComponent<Skill_Effect>().isProtect = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Destroy(other.gameObject);
        }
    }
}
