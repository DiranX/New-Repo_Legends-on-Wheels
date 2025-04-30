using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControlScene1 : MonoBehaviour 
{

 public AudioClip sfxButton;
 public int sceneIndex = 0;
 private bool oneshotSfx;
 
 // Update is called once per frame
 void Update () 
 {
    
    if ((Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame) || (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame))
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
        SceneManager.LoadScene(sceneIndex);
    }
 

}
