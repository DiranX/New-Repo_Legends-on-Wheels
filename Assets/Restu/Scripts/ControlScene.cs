using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControlScene : MonoBehaviour
{
    public AudioClip sfxButton;
    private bool oneshotSfx;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if ((Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame) ||
            (Keyboard.current != null && (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)))
        {
            if (!oneshotSfx)
            {
                oneshotSfx = true;
                StartCoroutine(PlaySfxThenLoadScene());
            }
        }
    }

    IEnumerator PlaySfxThenLoadScene()
    {
        audioSource.PlayOneShot(sfxButton);
        yield return new WaitForSeconds(sfxButton.length);
        SceneManager.LoadScene(1);
    }
}
