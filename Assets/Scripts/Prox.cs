using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Video;

public class Prox : MonoBehaviour
{
    public ProximitySensor proximity;
    public VideoPlayer videoPlayer;

    [Tooltip("Distance below which active video is triggered")]
    public float activationDistance = 0.5f;

    public string idleVideoName = "idle.mp4";
    public string activeVideoName = "active.mp4";

    private string currentVideo = "";

    private void Start()
    {
        if (ProximitySensor.current != null)
        {
            proximity = ProximitySensor.current;
            InputSystem.EnableDevice(proximity);
            Debug.Log("Proximity sensor is working");
        }
        else
        {
            Debug.LogError("Proximity sensor not found");
        }

        // Start with idle video
        PlayVideo(idleVideoName);
    }

    void Update()
    {
        if (proximity != null)
        {
            float distance = proximity.distance.ReadValue();

            if (distance < activationDistance)
            {
                if (currentVideo != activeVideoName)
                    PlayVideo(activeVideoName);
            }
            else
            {
                if (currentVideo != idleVideoName)
                    PlayVideo(idleVideoName);
            }
        }
        else
        {
            Debug.LogWarning("Proximity sensor is null");
        }
    }

    void PlayVideo(string videoName)
    {
        string path = System.IO.Path.Combine(Application.streamingAssetsPath, "Videos", videoName);
        path = path.Replace("\\", "/"); // force forward slashes

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        path = "file:///" + path;
#else
path = "file://" + path;
#endif


        videoPlayer.url = path;
        videoPlayer.isLooping = true;
        videoPlayer.Play();
        currentVideo = videoName;
        Debug.Log("Playing video: " + videoName);
    }
}
