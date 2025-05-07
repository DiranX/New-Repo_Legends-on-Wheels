using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private int refreshRate;

    void Start()
    {
        refreshRate = Screen.currentResolution.refreshRate;
        Debug.Log("Refresh Rate Monitor: " + refreshRate + " Hz");
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


}
