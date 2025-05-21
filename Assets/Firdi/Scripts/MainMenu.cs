using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public PlayerKartInput action;

    public GameObject pauseUi;
    public GameObject pauseButton;
    public GameObject backButton;
    public GameObject leaderBoard;

    public EventSystem eventSystem;

    private void Awake()
    {
        action = new PlayerKartInput();
        action.PlayerKart.Pause.performed += TooglePauseMenu; // Only performed to avoid issues
    }

    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
        action.Disable(); // Corrected from Enable()
    }

    void TooglePauseMenu(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        bool willShow = !pauseUi.activeSelf;
        pauseUi.SetActive(willShow);

        if (willShow)
        {
            Time.timeScale = 0;

            // Decide which button to highlight based on leaderboard visibility
            GameObject buttonToSelect = (leaderBoard != null && leaderBoard.activeSelf) ? backButton : pauseButton;
            StartCoroutine(SelectButtonNextFrame(buttonToSelect));
        }
        else
        {
            Time.timeScale = 1;
            eventSystem.SetSelectedGameObject(null);
        }
    }

    private IEnumerator SelectButtonNextFrame(GameObject button)
    {
        yield return null; // Wait one frame
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(button);
    }

    public void Dua()
    {
        GameManager.Instance.playerCount = 2;
        SceneManager.LoadSceneAsync("CharacterSelection2");
    }

    public void Tiga()
    {
        GameManager.Instance.playerCount = 3;
        SceneManager.LoadSceneAsync("CharacterSelection3");
    }

    public void Empat()
    {
        GameManager.Instance.playerCount = 4;
        SceneManager.LoadSceneAsync("CharacterSelection4");
    }

    public void Track(string track)
    {
        SceneManager.LoadSceneAsync(track);
    }

    public void Pause()
    {
        if (Time.timeScale >= 1)
        {
            Time.timeScale = 0;
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            pauseUi.SetActive(false);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
