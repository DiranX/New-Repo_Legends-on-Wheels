using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class QuitGame : MonoBehaviour
{
    public GameObject quitMenu;
    public GameObject controlScene;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame) ||
            (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame))
        {
            quitMenu.gameObject.SetActive(!quitMenu.gameObject.activeSelf);
        }

        controlScene.SetActive(!quitMenu.activeSelf);
    }
}


