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
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if ((Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame) ||
            (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame))
        {
            if (!oneshotSfx)
            {
                oneshotSfx = true;
                StartCoroutine(PlaySfxAndLoadScene());
            }
        }
    }

    IEnumerator PlaySfxAndLoadScene()
    {
        audioSource.PlayOneShot(sfxButton);
        yield return new WaitForSeconds(sfxButton.length);
        SceneManager.LoadScene(sceneIndex);
    }
}
