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
            ButoIjo_Skill buto = other.GetComponent<ButoIjo_Skill>();
            if(buto != null)
            {
                if(this.Id != buto.Id)
                {
                    other.gameObject.GetComponent<Animator>().SetTrigger("Stop");
                }
            }
        }
    }
}
