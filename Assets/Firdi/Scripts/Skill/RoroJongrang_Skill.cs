using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RoroJongrang_Skill : MonoBehaviour
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
    public GameObject Candi;
    public Transform Front;
    public Transform Back;
    // Start is called before the first frame update
    void Start()
    {
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
        Vector2 MoveY = playerKartKontroller.actions["Move"].ReadValue<Vector2>();
        UIcon.fillAmount = remainingTime / duration;
        if (remainingTime <= 0)
        {
            canUsed = true;
            if (skillUsed && canUsed && Time.time - lastUsedTime >= duration)
            {
                Debug.Log("Skill is Used");
                canUsed = false;
                lastUsedTime = Time.time;
                if (MoveY.y >= 0)
                {
                    GameObject candi = Instantiate(Candi, Front.position, Front.rotation);
                }
                else if (MoveY.y <= -0.5f)
                {
                    GameObject candi = Instantiate(Candi, Back.position, Back.rotation);
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
