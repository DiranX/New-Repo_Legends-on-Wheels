using UnityEngine;
using UnityEngine.InputSystem;

public class PanelToggle : MonoBehaviour
{
    public GameObject panel;
    public InputAction openPanelAction;  // Bind to Right Shoulder (RB)
    public InputAction closePanelAction; // Bind to Left Shoulder (LB)

    private void OnEnable()
    {
        openPanelAction.Enable();
        closePanelAction.Enable();

        openPanelAction.performed += OnOpenPanel;
        closePanelAction.performed += OnClosePanel;
    }

    private void OnDisable()
    {
        openPanelAction.performed -= OnOpenPanel;
        closePanelAction.performed -= OnClosePanel;

        openPanelAction.Disable();
        closePanelAction.Disable();
    }

    private void OnOpenPanel(InputAction.CallbackContext context)
    {
        if (panel != null)
            panel.SetActive(true);
    }

    private void OnClosePanel(InputAction.CallbackContext context)
    {
        if (panel != null)
            panel.SetActive(false);
    }
}
