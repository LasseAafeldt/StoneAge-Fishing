using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrackActivity
{
    bool GetIsActive(Queue<bool> que);
    void UpdateTracking(int length, int time, ActivityQueContainer container);
}
