using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AssignCanvasToCamera : MonoBehaviour
{
    private Canvas canvas;
    private Camera playerCamera;
    private PlayerInput playerInput;
    private CanvasScaler canvasScaler;
    public RectTransform P1P2UI;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvasScaler = GetComponent<CanvasScaler>();
        playerInput = GetComponentInParent<PlayerInput>(); // Get player input component

        if (playerInput != null)
        {
            playerCamera = playerInput.camera; // Get the camera assigned by Player Input Manager
            if (playerCamera != null)
            {
                canvas.worldCamera = playerCamera; // Assign player camera to canvas
            }
        }
    }

    void Start()
    {
        AdjustUIScale(); // Adjust UI size based on player count
    }

    void AdjustUIScale()
    {
        int playerCount = PlayerInput.all.Count; // Get total number of players

        if (canvasScaler != null)
        {
            if (playerCount == 2)
            {
                P1P2UI.sizeDelta = new Vector2(1350, 1550);
            }
            else if (playerCount == 4)
                canvasScaler.scaleFactor = 0.6f; // Even smaller UI for 4 players
            else
                canvasScaler.scaleFactor = 1f; // Default size for 1 player
        }
    }
}
