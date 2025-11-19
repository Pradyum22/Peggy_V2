using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class DisplayManager : MonoBehaviour
{
    public Canvas uiCanvas; // Drag your UI canvas here
    public VideoPlayer videoPlayer;

    void Start()
    {
        // Go fullscreen on app launch
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        Debug.Log("[DisplayManager] Set to fullscreen mode.");

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
            videoPlayer.started += OnVideoStarted;
        }
    }

    void OnVideoStarted(VideoPlayer vp)
    {
        if (uiCanvas != null)
        {
            uiCanvas.enabled = false;
            Debug.Log("[DisplayManager] UI Canvas hidden during video playback.");
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        if (uiCanvas != null)
        {
            uiCanvas.enabled = true;
            Debug.Log("[DisplayManager] UI Canvas re-enabled after video.");
        }
    }
}

