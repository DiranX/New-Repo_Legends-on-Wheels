using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButoIjo_Skill : MonoBehaviour
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
    public Transform skillSpawn;
    public GameObject sphere;
    Skill_Effect efek;
    public int Id;

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
        }
    }

    // Update is called once per frame
    void Update()
    {
        UIcon.fillAmount = remainingTime / duration;
        if (remainingTime <= 0)
        {
            canUsed = true;
            if (skillUsed && canUsed && Time.time - lastUsedTime >= duration)
            {
                GameObject obj = Instantiate(sphere, skillSpawn.position, Quaternion.identity);
                obj.GetComponent<ButoIjo_Shock>().Id = Id;
                canUsed = false;
                lastUsedTime = Time.time;
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
