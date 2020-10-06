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


    private void Start()
    {
        _vidPlayer = GetComponent<VideoPlayer>();
        _vidPlayer.loopPointReached += OnVideoEnd;
    }


    public void OnVideoEnd(VideoPlayer videoPlayer)
    {
        if (!hasEndEvent) return;
        //do an event
        if (VideoHasEnded != null)
        {
            VideoHasEnded.Invoke();
        }

    }

    public void PlayWithEndEvent()
    {
        hasEndEvent = true;
        _vidPlayer.Play();
    }
}
