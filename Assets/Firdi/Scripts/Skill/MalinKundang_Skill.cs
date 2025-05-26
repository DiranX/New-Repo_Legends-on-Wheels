using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MalinKundang_Skill : MonoBehaviour
{
    private PlayerInput playerKartKontroller;
    public GameObject UiSkill;
    private bool skillUsed;
    public bool canUsed;
    public Image UIcon;
    public GameObject Ready;
    public float duration;
    public float remainingTime;
    private float lastUsedTime = -Mathf.Infinity;
    float timeSinceUsed;
    Skill_Effect efek;
    public Texture[] texture;
    public Texture[] Chartexture;
    public Renderer[] Kartrender;
    public Renderer[] Chararender;
    int index;
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
        Renderer();
        UIcon.fillAmount = 1f - (remainingTime / duration);
        if (remainingTime <= 0)
        {
            canUsed = true;
            Ready.SetActive(true);
            if (skillUsed && canUsed && Time.time - lastUsedTime >= duration)
            {
                canUsed = false;
                lastUsedTime = Time.time;
                efek.isProtect = true;
                index = 1;
                StartCoroutine(Timeup());
            }
        }
        else
        {
            canUsed = false;
            Ready.SetActive(false);
            StopCoroutine(Timeup());
        }

        timeSinceUsed = Time.time - lastUsedTime;
        remainingTime = Mathf.Max(0, duration - timeSinceUsed);

    }

    void Renderer()
    {
        foreach (var item in Kartrender)
        {
            item.material.mainTexture = texture[index];
        }
        if(index == 1)
        {
            Chararender[0].material.mainTexture = Chartexture[9];
            Chararender[1].material.mainTexture = Chartexture[9];
            Chararender[2].material.mainTexture = Chartexture[9];
            Chararender[3].material.mainTexture = Chartexture[9];
            Chararender[4].material.mainTexture = Chartexture[9];
            Chararender[5].material.mainTexture = Chartexture[9];
            Chararender[6].material.mainTexture = Chartexture[9];
            Chararender[7].material.mainTexture = Chartexture[9];
            Chararender[8].material.mainTexture = Chartexture[9];
        }
    }

    IEnumerator Timeup()
    {
        yield return new WaitForSeconds(10);
        index = 0;

        if(index == 0)
        {
            Chararender[0].material.mainTexture = Chartexture[0];
            Chararender[1].material.mainTexture = Chartexture[1];
            Chararender[2].material.mainTexture = Chartexture[2];
            Chararender[3].material.mainTexture = Chartexture[3];
            Chararender[4].material.mainTexture = Chartexture[4];
            Chararender[5].material.mainTexture = Chartexture[5];
            Chararender[6].material.mainTexture = Chartexture[6];
            Chararender[7].material.mainTexture = Chartexture[7];
            Chararender[8].material.mainTexture = Chartexture[8];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(index == 1)
        {
            if (other.gameObject.CompareTag("Obstacle"))
            {
                Destroy(other.gameObject);
            }
        }
    }
}