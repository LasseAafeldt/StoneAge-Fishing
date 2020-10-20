using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackHeadRotation : MonoBehaviour, ITrackActivity
{
    [SerializeField] private float thresholdAnglePerSecond = 0.01f;


    public bool GetIsActive(Queue<bool> que)
    {
        for (int i = que.Count - 1; i >= 0; i--)
        {
            if (que.ToArray()[i] == false)
            {
                return true;
            }
        }

        return false;
    }

    public void UpdateTracking(int queSizeSeconds, int checksPerSecond, ActivityQueContainer container)
    {
        Queue<bool> que = container.activityQue;
        if (!Input.GetButton("Fire1"))
        {
            Quaternion currentRead = GvrVRHelpers.GetHeadRotation();
            //makes sure that we don't overflow the que
            if (que.Count >= queSizeSeconds * checksPerSecond)
            {
                que.Dequeue();
            }
            if (container.lastRead == null)
                que.Enqueue(false);

            if (Quaternion.Angle(container.lastRead, currentRead) >= thresholdAnglePerSecond / checksPerSecond)
            {
                que.Enqueue(false);
            }
            else
            {
                que.Enqueue(true);
            }

            container.lastRead = currentRead;
        }
        else
        {
            que.Enqueue(false);
        }
    }
        
}
