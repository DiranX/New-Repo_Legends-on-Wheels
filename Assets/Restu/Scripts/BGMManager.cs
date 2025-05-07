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
    public AudioClip characterSelect2;
    public AudioClip characterSelect3;
    public AudioClip characterSelect4;

    public AudioClip arenaSelectMusic;

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
                ChangeMusic(titleScreen);
                break;
            case "Settings":
                ChangeMusic(settings);
            break;
            case "Credits":
                ChangeMusic(credits);
            break;
            case "ModeSelect":
                ChangeMusic(titleScreen);
                break;
            case "CharacterSelect2":
                ChangeMusic(characterSelect2);
                break;
            case "CharacterSelect3":
                ChangeMusic(characterSelect2);
                break;
            case "CharacterSelect4":
                ChangeMusic(characterSelect2);
                break;
            case "ArenaSelect":
                ChangeMusic(characterSelect2); // Arena pakai musik dari Character Select
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
