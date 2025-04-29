using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Camera_GroundCheck : MonoBehaviour
{
    public Transform Camera;  // Assign your player's kart
    public float forwardOffset;  // Distance in front of the kart
    public LayerMask groundLayer;  // Assign the ground layer in Inspector
    private float lastValidHeight; // Store last valid height to prevent sinking
    public CinemachineVirtualCamera virtualCamera;
    public float distancefar;
    public float distance;
    private float yVelocity = 0f;
    private void Start()
    {
        if (Camera != null)
        {
            Vector3 offset = transform.position - Camera.position;
            forwardOffset = Vector3.Dot(offset, Camera.forward);
        }

    }

    void Update()
    {
        Aligning();

        distance = Vector3.Distance(this.transform.position, virtualCamera.transform.position);

        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        // inside Update:
        if (transposer != null)
        {
            float targetHeight = distance < distancefar ? 5f : 3f;

            Vector3 currentOffset = transposer.m_FollowOffset;
            currentOffset.y = Mathf.SmoothDamp(currentOffset.y, targetHeight, ref yVelocity, .7f);
            transposer.m_FollowOffset = currentOffset;
        }
    }


    void Aligning()
    {
        if (!Camera) return;

        // Set position in front of kart
        Vector3 forwardPosition = Camera.position + Camera.forward * forwardOffset;
        // Set a high starting point for the raycast (above the kart)
        Vector3 rayOrigin = forwardPosition + Vector3.up * 20;

        // Raycast down from this position to find the ground
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin + Vector3.up, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            // Snap to ground
            transform.position = hit.point;
            lastValidHeight = hit.point.y;
        }
        else
        {
            // If no ground detected, keep it at the original position (failsafe)
            transform.position = new Vector3(forwardPosition.x, lastValidHeight, forwardPosition.z);
        }

        // Keep rotation aligned with the kart
        transform.rotation = Quaternion.Euler(0, Camera.eulerAngles.y, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(this.transform.position, 1);
    }
}
