using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPlayerManager : MonoBehaviour
{

    public float yieldTime =2;

    [SerializeField] private VideoControls videoControls;

    private void Awake()
    {
        if (!videoControls)
        {
            videoControls = GetComponentInChildren<VideoControls>();
        }
       
    }

    public void StartVideoWithDelay()
    {
        StartCoroutine(StartVideoCoroutine(yieldTime));
    }

    IEnumerator StartVideoCoroutine(float yieldTime)
    {
        yield return new WaitForSeconds(yieldTime);
        videoControls.gameObject.SetActive(true);
        videoControls.PlayWithEndEvent();
    }
}
