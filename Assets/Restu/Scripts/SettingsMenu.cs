using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private int refreshRate;
    private AudioSource BGM;
    private AudioSource SFX;
    public bool BGMActive;
    public bool SFXActive;

    void Start()
    {
        refreshRate = Screen.currentResolution.refreshRate;
        Debug.Log("Refresh Rate Monitor: " + refreshRate + " Hz");

        BGM = GameObject.FindGameObjectWithTag("BGM Tag").GetComponent<AudioSource>();
        BGM.enabled = true;
        SFX = GameObject.FindGameObjectWithTag("SFX Tag").GetComponent<AudioSource>();
        SFX.enabled = true;
    }


    void Update() 
    {
        Debug.Log("FPS: " + (1f / Time.unscaledDeltaTime));
    }

    public void SetVsync(bool isVsync)
    {
        if (isVsync)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = refreshRate;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = -1;
        }
    }

    // Start is called before the first frame update
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void ToggleBGM(bool isActive)
    {
        BGMActive = isActive;
        BGM.enabled = BGMActive;
    }

    public void ToggleSFX(bool isActive)
    {
        SFXActive = isActive;
        SFX.enabled = SFXActive;
    }


}
