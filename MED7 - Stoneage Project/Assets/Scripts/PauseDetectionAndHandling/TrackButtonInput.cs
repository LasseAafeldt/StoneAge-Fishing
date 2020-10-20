using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class TrackButtonInput : MonoBehaviour, ITrackActivity
{

    private bool isActive;

    private void Start()
    {
        isActive = false;
    }


    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            SetIsActiveTrue();
        }
    }

    private void SetIsActiveTrue()
    {
        isActive = true;
    }

    public bool GetIsActive(Queue<bool> que)
    {
        return isActive;
    }

    public void UpdateTracking(int length, int time, ActivityQueContainer container)
    {
        //hehe
        //no queue needs to be updated when we use button input
    }
}
