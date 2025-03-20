using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Effect : MonoBehaviour
{
    [Header("Blind Effect")]
    public GameObject lumpur;
    bool isblind;
    private void Update()
    {
        StartCoroutine(BlindingEffect());
        if (!lumpur.activeSelf)
        {
            StopCoroutine(BlindingEffect());
        }
    }
    IEnumerator BlindingEffect()
    {
        if (lumpur.activeSelf && !isblind)
        {
            isblind = true;
            yield return new WaitForSeconds(10);
            lumpur.SetActive(false);
            isblind = false;
        }
    }
}
