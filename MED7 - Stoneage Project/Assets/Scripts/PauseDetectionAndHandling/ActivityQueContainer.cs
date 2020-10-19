using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActivityQueContainer
{
    public static Queue<bool> activityQue;
    public static Quaternion lastRead;

    public static void InstantiateQue(int queLength)
    {
        activityQue = new Queue<bool>(queLength);
        for (int i = 0; i < activityQue.Count; i++)
        {
            activityQue.Enqueue(false);
        }
    }

}
