using UnityEngine;
using UnityEngine.UI;

public class ToggleHandler : MonoBehaviour
{
    [Header("Assign Toggles Here")]
    public Toggle sfxToggle;
    public Toggle bgmToggle;
    public Toggle fullscreenToggle;
    public Toggle vsyncToggle;

    private void Start()
    {
        SetupToggle(sfxToggle, "SFXToggle");
        SetupToggle(bgmToggle, "BGMToggle");
        SetupToggle(fullscreenToggle, "FullscreenToggle");
        SetupToggle(vsyncToggle, "VSYNCToggle");
    }

    private void SetupToggle(Toggle toggle, string key)
    {
        if (toggle != null)
        {
            // Apply saved state on start
            bool savedState = PlayerPrefs.GetInt(key, 0) == 1;
            toggle.isOn = savedState;

            // Remove previous listeners to avoid duplicates
            toggle.onValueChanged.RemoveAllListeners();

            // Save toggle state when changed
            toggle.onValueChanged.AddListener((isOn) =>
            {
                PlayerPrefs.SetInt(key, isOn ? 1 : 0);
                PlayerPrefs.Save();
                Debug.Log($"Toggle '{key}' saved as: {isOn}");
            });
        }
    }

    // Optional: Manual toggle switchers (like TurnOffToggle)
    public void ToggleSFX()
    {
        if (sfxToggle != null) sfxToggle.isOn = !sfxToggle.isOn;
    }

    public void ToggleBGM()
    {
        if (bgmToggle != null) bgmToggle.isOn = !bgmToggle.isOn;
    }

    public void ToggleFullscreen()
    {
        if (fullscreenToggle != null) fullscreenToggle.isOn = !fullscreenToggle.isOn;
    }

    public void ToggleVSYNC()
    {
        if (vsyncToggle != null) vsyncToggle.isOn = !vsyncToggle.isOn;
    }
}
