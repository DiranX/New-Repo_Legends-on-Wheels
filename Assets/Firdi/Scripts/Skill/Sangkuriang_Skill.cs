using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Sangkuriang_Skill : MonoBehaviour
{
    private PlayerInput playerKartKontroller;
    public GameObject UiSkill;
    private bool skillUsed;
    public bool canUsed;
    public Image UIcon;
    public float duration;
    public float remainingTime;
    private float lastUsedTime = -Mathf.Infinity;
    float timeSinceUsed;
    Skill_Effect efek;
    public GameObject Bukit;
    public Transform Front;
    public Transform Back;
    public int Id;
    void Start()
    {
        efek = GetComponent<Skill_Effect>();
        playerKartKontroller = GetComponentInParent<PlayerInput>();
        playerKartKontroller.actions["Skill"].started += ctx => skillUsed = true;
        playerKartKontroller.actions["Skill"].canceled += ctx => skillUsed = false;
        if (this.gameObject.activeSelf)
        {
            UiSkill.SetActive(true);
            //this.Id = GetComponent<PlayerKartController>().ID;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 MoveY = playerKartKontroller.actions["Move"].ReadValue<Vector2>();
        UIcon.fillAmount = remainingTime / duration;
        if (remainingTime <= 0)
        {
            canUsed = true;
            if (skillUsed && canUsed && Time.time - lastUsedTime >= duration)
            {
                canUsed = false;
                lastUsedTime = Time.time;
                efek.isProtect = true;
                if (MoveY.y >= 0)
                {
                    GameObject bukit = Instantiate(Bukit, Front.position, Front.rotation);
                    bukit.GetComponent<Sangkuriang_Bukit>().Id = this.Id;
                }
                else if (MoveY.y <= -0.5f)
                {
                    GameObject bukit = Instantiate(Bukit, Back.position, Back.rotation);
                    bukit.GetComponent<Sangkuriang_Bukit>().Id = this.Id;
                }
            }
        }
        else
        {
            canUsed = false;
        }

        timeSinceUsed = Time.time - lastUsedTime;
        remainingTime = Mathf.Max(0, duration - timeSinceUsed);

    }
}
