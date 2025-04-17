using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NyiroroKidul_Tornado : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerItemHolder>().playerKartController.GetComponent<Animator>().SetTrigger("Stop");
            Debug.Log("Tornado Hit Player");
        }

        if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit Obstacle");
            Destroy(other.gameObject);
        }
    }
}
