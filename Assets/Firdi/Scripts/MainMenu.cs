using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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

    public void Track()
    {
        SceneManager.LoadSceneAsync("Track1");
    }
    public void Track2()
    {
        SceneManager.LoadSceneAsync("Track2");
    }
}
