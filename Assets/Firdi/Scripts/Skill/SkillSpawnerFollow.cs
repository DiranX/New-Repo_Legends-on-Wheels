using UnityEngine;

public class SkillSpawnerFollow : MonoBehaviour
{
    public Transform playerKart;  // Assign your player's kart
    public float forwardOffset;  // Distance in front of the kart
    public LayerMask groundLayer;  // Assign the ground layer in Inspector
    private float lastValidHeight; // Store last valid height to prevent sinking

    void Update()
    {
        if (!playerKart) return;

        // Set position in front of kart
        Vector3 forwardPosition = playerKart.position + playerKart.forward * forwardOffset;
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
        transform.rotation = Quaternion.Euler(0, playerKart.eulerAngles.y, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(this.transform.position, 1);
    }
}
