using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackHeadRotation : MonoBehaviour, ITrackActivity
{
    [SerializeField] private float thresholdAnglePerSecond = 2;

    Queue<bool> activityQue = ActivityQueContainer.activityQue;

    public bool GetIsActive()
    {
        for (int i = activityQue.Count - 1; i >= 0; i--)
        {
            if (activityQue.ToArray()[i] == false)
                return true;
        }
        return false;
    }

    public void UpdateTracking(int queSizeSeconds, int checksPerSecond)
    {
        Quaternion currentRead = GvrVRHelpers.GetHeadRotation();
        //makes sure that we don't overflow the que
        if (activityQue.Count >= queSizeSeconds * checksPerSecond)
        {
            activityQue.Dequeue();
        }
        if(ActivityQueContainer.lastRead == null)
            activityQue.Enqueue(false);

        if(Quaternion.Angle(ActivityQueContainer.lastRead, currentRead)>= thresholdAnglePerSecond / checksPerSecond)
        {
            activityQue.Enqueue(false);
        }
        else
        {
            activityQue.Enqueue(true);
        }

        ActivityQueContainer.lastRead = currentRead;
    }
}
