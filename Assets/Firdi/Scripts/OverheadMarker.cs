using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheadMarker : MonoBehaviour
{
    public GameObject markerPrefab; // Your overhead marker UI prefab
    public List<Transform> playerKarts; // All 4 player karts
    public List<Camera> playerCameras;  // All 4 player cameras

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            Camera cam = playerCameras[i];
            Transform canvasTransform = cam.GetComponentInChildren<Canvas>().transform;

            for (int j = 0; j < 4; j++)
            {
                if (i == j) continue; // Skip self

                GameObject marker = Instantiate(markerPrefab, canvasTransform);
                var follow = marker.GetComponent<OverheadUI>();
                follow.cam = cam;
                follow.target = playerKarts[j];
            }
        }
    }
}
