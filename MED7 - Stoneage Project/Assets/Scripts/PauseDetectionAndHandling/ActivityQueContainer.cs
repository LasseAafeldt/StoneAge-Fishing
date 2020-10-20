using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityQueContainer
{
    public Queue<bool> activityQue;
    public Quaternion lastRead;

    public void InstantiateQue(int queLength, bool value)
    {
        activityQue = new Queue<bool>(queLength);
        for (int i = 0; i < activityQue.Count; i++)
        {
            activityQue.Enqueue(value);
        }
    }

}
