using UnityEngine;
using UnityEngine.Video;
using System.Collections.Generic;

public class VideoSwitcher : MonoBehaviour
{
    [Header("Video Player & Clips")]
    public VideoPlayer videoPlayer;
    public List<VideoClip> videoClips; // Assign in Inspector

    public void PlayVideoById(int id)
    {
        if (id < 0 || id >= videoClips.Count)
        {
            Debug.LogWarning("[VideoSwitcher] Invalid video index.");
            return;
        }

        videoPlayer.clip = videoClips[id];
        Debug.Log($"[VideoSwitcher] Assigned Clip: {videoClips[id].name}");

        videoPlayer.Prepare(); // required to load video before playback

        videoPlayer.prepareCompleted += (vp) =>
        {
            Debug.Log("[VideoSwitcher] Video prepared. Starting playback.");
            vp.Play();
        };

        videoPlayer.errorReceived += (vp, msg) => {
            Debug.LogError("[VideoPlayer Error] " + msg);
        };

    }

}
