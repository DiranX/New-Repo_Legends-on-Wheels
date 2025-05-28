using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControlScene1 : MonoBehaviour
{
    public AudioClip sfxButton;
    public int sceneIndex = 0;
    private bool oneshotSfx = false;
    private AudioSource audioSource;

    void Start()
    {
        // Tambahkan AudioSource ke GameObject ini
        audioSource = GameObject.FindGameObjectWithTag("SFX Tag").GetComponent<AudioSource>();
    }

    void Update()
    {
        if ((Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame) ||
            (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame))
        {
            if (!oneshotSfx && this != null)
            {
                oneshotSfx = true;
                StartCoroutine(PlaySfxAndLoadScene());
                SceneManager.LoadScene(sceneIndex);
            }
        }
    }

    IEnumerator PlaySfxAndLoadScene()
    {
        if (audioSource.enabled == true)
        {
            audioSource.PlayOneShot(sfxButton);
            yield return new WaitForSeconds(sfxButton.length);
        }
    }
}
