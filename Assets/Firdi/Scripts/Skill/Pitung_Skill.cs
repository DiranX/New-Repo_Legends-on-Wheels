using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Pitung_Skill : MonoBehaviour
{
    private PlayerInput playerKartKontroller;
    private bool skillUsed;
    public bool canUsed;
    public Transform frontThrow;
    public Transform backThrow;
    public Image UIcon;
    public float duration;
    public float remainingTime;
    private float lastUsedTime = -Mathf.Infinity;
    float timeSinceUsed;
    public GameObject Golok;
    public GameObject UiSkill;
    public float Force;
    public int Id;

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

        this.Id = GetComponentInParent<Player>().id;

    }

    private void FixedUpdate()
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
                    FrontThrow();
                }
                else if (MoveY.y <= -0.5)
                {
                    BackThrow();
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

    void FrontThrow()
    {
        GameObject golok = Instantiate(Golok, frontThrow.position, frontThrow.rotation);
        Rigidbody rb = golok.GetComponent<Rigidbody>();
        golok.GetComponent<Pitung_Golok>().pitung = this.GetComponent<Pitung_Skill>();
        golok.GetComponent<Pitung_Golok>().Id = this.Id;

        Vector3 Direction = frontThrow.forward * Force;

        Vector3 playerVelocity = GetComponent<PlayerKartController>().sphere.GetComponent<Rigidbody>().velocity;

        // Calculate force multiplier based on speed
        float speedFactor = Mathf.Clamp(playerVelocity.magnitude / 10f, 0.5f, 2f); // Adjust range as needed

        // Apply dynamic force
        Vector3 throwDirection = frontThrow.forward * (Force * speedFactor);
        rb.AddForce(throwDirection, ForceMode.Impulse);
    }

    void BackThrow()
    {
        GameObject golok = Instantiate(Golok, backThrow.position, backThrow.rotation);
        Rigidbody rb = golok.GetComponent<Rigidbody>();

        Vector3 Direction = backThrow.forward * Force;

        Vector3 playerVelocity = GetComponent<PlayerKartController>().sphere.GetComponent<Rigidbody>().velocity;

        // Calculate force multiplier based on speed
        float speedFactor = Mathf.Clamp(playerVelocity.magnitude / 10f, 0.5f, 2f); // Adjust range as needed

        // Apply dynamic force
        Vector3 throwDirection = -backThrow.forward * (Force * speedFactor / 2);
        rb.AddForce(throwDirection, ForceMode.Impulse);
    }
}
