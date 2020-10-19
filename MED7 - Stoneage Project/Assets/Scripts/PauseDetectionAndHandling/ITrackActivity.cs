using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrackActivity
{
    bool GetIsActive();
    void UpdateTracking(int length, int time);
}
