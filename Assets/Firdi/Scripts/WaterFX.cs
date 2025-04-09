using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WaterFX : MonoBehaviour
{
    [SerializeField] GameObject waterFog;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            //waterfx.gameObject.SetActive(true);
            //RenderSettings.fog = true;
            waterFog.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            //waterfx.gameObject.SetActive(false);
            //RenderSettings.fog = false;
            waterFog.SetActive(false);
        }
    }
}
