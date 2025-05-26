using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class Pitung_Golok : MonoBehaviour
{
    public int Id;
    public bool isBack;
    public Pitung_Skill pitung;
    public float speed = 5f;
    public Vector3 velocity;
    public LayerMask terrain;
    private float lastValidHeight;
    public float offsetAboveGround = 0.5f;
    int bounceCount;
    public bool trigger;

    private void Start()
    {
        Physics.IgnoreLayerCollision(3, 11, true);
        StartCoroutine(Wait());
    }
    void Update()
    {
        // Set a high starting point for the raycast (above the kart)
        Vector3 rayOrigin = transform.position + Vector3.up * 20;

        // Raycast down from this position to find the ground
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin + Vector3.up, Vector3.down, out hit, Mathf.Infinity, terrain))
        {
            // Move the object slightly above the ground
            transform.position = hit.point + Vector3.up * offsetAboveGround;

            lastValidHeight = hit.point.y;
        }
        else
        {
            // If no ground detected, keep it at the original position (failsafe)
            transform.position = new Vector3(transform.position.x, lastValidHeight, transform.position.z);
        }

        transform.position += velocity * speed * Time.deltaTime;

        if (bounceCount >= 20)
        {
            Destroy(gameObject);
        }
        StartCoroutine(Destroyit());
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            float zHitOffset = transform.position.z - collision.transform.position.z;
            float maxwallHalfHeight = collision.collider.bounds.size.z / 2;
            float normalizedZ = Mathf.Clamp(zHitOffset / maxwallHalfHeight, -1f, 1f);

            velocity = new Vector3(-velocity.x, 0f, normalizedZ).normalized * ((speed / 2) * 3);
            bounceCount += 1;
        }
    }

    IEnumerator Destroyit()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        Physics.IgnoreLayerCollision(3, 11, false);
    }
}
