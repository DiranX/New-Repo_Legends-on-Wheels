using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    public static BGMManager instance;

    [Header("List Musik Per Scene")]
    public AudioClip titleScreen;
    public AudioClip mainMenu;
    public AudioClip settings;
    public AudioClip credits;
    public AudioClip modeSelect;
    public AudioClip characterSelection2;
    public AudioClip characterSelection3;
    public AudioClip characterSelection4;
    public AudioClip arenaSelection;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        musicSource.Play(); // Kalau mau play musik awal
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cek nama scene dan ganti musik sesuai kebutuhan
        switch (scene.name)
        {
            case "TitleScreen":
                ChangeMusic(titleScreen);
                break;
            case "MainMenu":
                ChangeMusic(mainMenu);
                break;
            case "Settings":
                ChangeMusic(settings);
            break;
            case "Credits":
                ChangeMusic(credits);
            break;
            case "ModeSelect":
                ChangeMusic(modeSelect);
                break;
            case "CharacterSelection2":
                ChangeMusic(characterSelection2);
                break;
            case "CharacterSelection3":
                ChangeMusic(characterSelection3);
                break;
            case "CharacterSelection4":
                ChangeMusic(characterSelection4);
                break;
            case "ArenaSelection":
                ChangeMusic(arenaSelection);
                break;
            default:
                // Kalau scene tidak spesifik, bisa pilih diam, stop musik, atau play musik default
                break;
        }
    }

    public void ChangeMusic(AudioClip newClip)
    {
        if (musicSource.clip == newClip) return; // Kalau sudah clip yang sama, tidak perlu restart

        musicSource.clip = newClip;
        musicSource.Play();
    }
}
