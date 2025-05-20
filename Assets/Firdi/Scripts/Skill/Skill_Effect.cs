using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Skill_Effect : MonoBehaviour
{
    [Header("Blind Effect")]
    public GameObject lumpur;
    public GameObject roroKidulRelocate;
    bool isblind;
    public bool isProtect;
    public bool isSlowed;
    public bool isReverse;
    private PlayerKartController kart;
    public float originalTopSpeed;
    private Coroutine slowDownRoutine;

    private void Start()
    {
        kart = GetComponent<PlayerKartController>();
        originalTopSpeed = kart.topSpeed;
    }

    private void Update()
    {
        StartCoroutine(BlindingEffect());
        if (!lumpur.activeSelf)
        {
            StopCoroutine(BlindingEffect());
        }
        if (isSlowed && slowDownRoutine == null)
        {
            slowDownRoutine = StartCoroutine(SlowDownEffect());
        }
        if (isSlowed == false)
        {
            StopCoroutine(SlowDownEffect());
        }
        StartCoroutine(ReverseEffect());
        if (isReverse == false)
        {
            StopCoroutine (ReverseEffect());
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
    IEnumerator SlowDownEffect()
    {
        yield return new WaitForSeconds(4f);
        kart.topSpeed = originalTopSpeed;
        isSlowed = false;
        slowDownRoutine = null;
    }
    IEnumerator ReverseEffect()
    {
        if (isReverse)
        {
            yield return new WaitForSeconds(3);
            isReverse = false;
        }
    }
}
