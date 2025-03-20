using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TimunEmas_Skill : MonoBehaviour
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
    public GameObject Terasi;
    public float Force;
    public float ForceY;

    // Start is called before the first frame update
    void Start()
    {
        playerKartKontroller = GetComponentInParent<PlayerInput>();

        playerKartKontroller.actions["Skill"].started += ctx => skillUsed = true;
        playerKartKontroller.actions["Skill"].canceled += ctx => skillUsed = false;

    }

    private void FixedUpdate()
    {
        Vector2 MoveY = playerKartKontroller.actions["Move"].ReadValue<Vector2>();
        UIcon.fillAmount = remainingTime/duration;
        if (remainingTime <= 0)
        {
            canUsed = true;
            if(skillUsed && canUsed && Time.time - lastUsedTime >= duration)
            {
                Debug.Log("Skill is Used");
                canUsed = false;
                lastUsedTime = Time.time;
                if (MoveY.y >= 0)
                {
                    FrontThrow();
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
        GameObject terasi = Instantiate(Terasi, frontThrow.position, frontThrow.rotation);
        Rigidbody rb = terasi.GetComponent<Rigidbody>();

        Vector3 Direction = frontThrow.forward * Force + Vector3.up * ForceY;

        Vector3 playerVelocity = GetComponent<PlayerKartController>().sphere.GetComponent<Rigidbody>().velocity;

        // Calculate force multiplier based on speed
        float speedFactor = Mathf.Clamp(playerVelocity.magnitude / 10f, 0.5f, 2f); // Adjust range as needed

        // Apply dynamic force
        Vector3 throwDirection = frontThrow.forward * (Force * speedFactor) + Vector3.up * (ForceY);
        rb.AddForce(throwDirection, ForceMode.Impulse);
    }
}
