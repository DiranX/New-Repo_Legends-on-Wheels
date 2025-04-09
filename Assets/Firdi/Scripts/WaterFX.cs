using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFX : MonoBehaviour
{
    [SerializeField] GameObject waterfx;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            //waterfx.gameObject.SetActive(true);
            RenderSettings.fog = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            //waterfx.gameObject.SetActive(false);
            RenderSettings.fog = false;
        }
    }
}
