using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoControls : MonoBehaviour
{
    private VideoPlayer _vidPlayer;
    public bool hasEndEvent = false;

    public delegate void VideoEnd();
    public event VideoEnd VideoHasEnded;

    private void Awake()
    {
        _vidPlayer = GetComponent<VideoPlayer>();
    }

    private void Start()
    {
        _vidPlayer.loopPointReached += OnVideoEnd;
    }


    public void OnVideoEnd(VideoPlayer videoPlayer)
    {
        Debug.Log("entered OnvideoEnd");
        if (!hasEndEvent) return;
        //do an event
        if (VideoHasEnded != null)
        {
            Debug.Log("Invoke VideoHasEnded");
            VideoHasEnded.Invoke();
        }

    }

    public void PlayWithEndEvent()
    {
        hasEndEvent = true;
        _vidPlayer.Play();
    }
}
