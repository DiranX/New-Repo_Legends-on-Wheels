using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class Kancil_Skill : MonoBehaviour
{
    private PlayerInput playerKartKontroller;
    public GameObject UiSkill;
    private bool skillUsed;
    public bool canUsed;
    public Image BoostCon;
    public Image UIcon;
    public GameObject cool;
    public float charge;
    public float duration;
    public float remainingTime;
    private float lastUsedTime = -Mathf.Infinity;
    float timeSinceUsed;
    Skill_Effect efek;

    // Start is called before the first frame update
    void Start()
    {
        efek = GetComponent<Skill_Effect>();
        playerKartKontroller = GetComponentInParent<PlayerInput>();
        playerKartKontroller.actions["Skill"].started += ctx => skillUsed = true;
        playerKartKontroller.actions["Skill"].canceled += ctx => skillUsed = false;
        if (this.gameObject.activeSelf)
        {
            UiSkill.SetActive(true);
            canUsed = false;
            lastUsedTime = Time.time;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UIcon.fillAmount = 1f - (remainingTime / duration);
        if (remainingTime <= 0)
        {
            canUsed = true;
            cool.SetActive(true);
            if (skillUsed && canUsed && Time.time - lastUsedTime >= duration)
            {
                canUsed = false;
                lastUsedTime = Time.time;
                GetComponent<PlayerKartController>().ReceiveBoost(1f, 3.5f);
            }
            //cool.SetActive(false);
            //if (charge >= 0)
            //{
            //    //BoostCon.fillAmount = charge / 100;
                
            //}else if(charge <= 0)
            //{
            //    if(canUsed) charge = 100;
            //}
        }
        else
        {
            canUsed = false;
            cool.SetActive(false);
        }

        timeSinceUsed = Time.time - lastUsedTime;
        remainingTime = Mathf.Max(0, duration - timeSinceUsed);
    }
}
