using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheadUI : MonoBehaviour
{
    public Transform target;          // The kart this marker follows
    public GameObject player;
    public Camera cam;               // The camera this marker uses (per player)
    public Vector3 offset = new Vector3(0, 2f, 0); // Height offset above the kart

    private RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        target = player.GetComponent<PlayerItemHolder>().playerKartController.transform;
    }

    void Update()
    {
        if (target == null || cam == null)
            return;

        Vector3 worldPos = target.position + offset;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

        // If behind camera, hide
        if (screenPos.z < 0)
        {
            gameObject.SetActive(false);
            return;
        }

        // Optionally clamp to screen bounds here

        gameObject.SetActive(true);
        rect.position = screenPos;
    }
}
