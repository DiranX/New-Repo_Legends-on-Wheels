using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControlScene : MonoBehaviour {

 public AudioClip sfxButton;
 
 private bool oneshotSfx;
 
 // Update is called once per frame
 void Update () 
 {
  
    //if press any key jump to gameplay scene
    if ((Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame) || (Keyboard.current != null && (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)))
    {
        if(!oneshotSfx)
        {
            AudioSource.PlayClipAtPoint(sfxButton,Vector3.zero);
            Invoke("LoadScene",0.5f);
            oneshotSfx = true;
        }
   
   
    }
 
 }
 
    void LoadScene()
    {
        SceneManager.LoadScene(1);
    }
 
}
