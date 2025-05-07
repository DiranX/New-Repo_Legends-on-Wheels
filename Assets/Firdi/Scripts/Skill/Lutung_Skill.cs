using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Lutung_Skill : MonoBehaviour
{
    private PlayerInput playerKartKontroller;
    private bool skillUsed;
    public bool canUsed;
    public Transform frontThrow;
    public Transform backThrow;
    public Image UIcon;
    public GameObject Ready;
    public float duration;
    public float remainingTime;
    private float lastUsedTime = -Mathf.Infinity;
    float timeSinceUsed;
    public GameObject Pisang;
    public GameObject UiSkill;
    public float Force;
    public float ForceY;
    public int id;

    // Start is called before the first frame update
    void Start()
    {
        playerKartKontroller = GetComponentInParent<PlayerInput>();

        playerKartKontroller.actions["Skill"].started += ctx => skillUsed = true;
        playerKartKontroller.actions["Skill"].canceled += ctx => skillUsed = false;

        if (this.gameObject.activeSelf)
        {
            UiSkill.SetActive(true);
            this.id = GetComponentInParent<Player>().id;
        }

    }

    private void FixedUpdate()
    {
        Vector2 MoveY = playerKartKontroller.actions["Move"].ReadValue<Vector2>();
        UIcon.fillAmount = 1f - (remainingTime / duration);
        if (remainingTime <= 0)
        {
            canUsed = true;
            Ready.SetActive(true);
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
            Ready.SetActive(false);
        }

        timeSinceUsed = Time.time - lastUsedTime;
        remainingTime = Mathf.Max(0, duration - timeSinceUsed);
    }

    void FrontThrow()
    {
        GameObject pis = Instantiate(Pisang, frontThrow.position, frontThrow.rotation);
        Rigidbody rb = pis.GetComponent<Rigidbody>();
        pis.GetComponent<Lutung_Pisang>().Id = this.id;

        Vector3 Direction = frontThrow.forward * Force + Vector3.up * ForceY;

        Vector3 playerVelocity = GetComponent<PlayerKartController>().sphere.GetComponent<Rigidbody>().velocity;

        // Calculate force multiplier based on speed
        float speedFactor = Mathf.Clamp(playerVelocity.magnitude / 10f, 0.5f, 2f); // Adjust range as needed

        // Apply dynamic force
        Vector3 throwDirection = frontThrow.forward * (Force * speedFactor) + Vector3.up * (ForceY);
        rb.AddForce(throwDirection, ForceMode.Impulse);
    }

    void BackThrow()
    {
        GameObject pis = Instantiate(Pisang, backThrow.position, backThrow.rotation);
        Rigidbody rb = pis.GetComponent<Rigidbody>();
        pis.GetComponent<Lutung_Pisang>().Id = this.id;

        Vector3 Direction = backThrow.forward * Force + Vector3.up * ForceY;

        Vector3 playerVelocity = GetComponent<PlayerKartController>().sphere.GetComponent<Rigidbody>().velocity;

        // Calculate force multiplier based on speed
        float speedFactor = Mathf.Clamp(playerVelocity.magnitude / 10f, 0.5f, 2f); // Adjust range as needed

        // Apply dynamic force
        Vector3 throwDirection = -backThrow.forward * (Force * speedFactor / 2) + Vector3.up * (ForceY);
        rb.AddForce(throwDirection, ForceMode.Impulse);
    }
}
