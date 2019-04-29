using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogFile {

    public List<string> dateAndTime = new List<string>();
    public List<Vector3> positions = new List<Vector3>();
    public List<Vector3> rotations = new List<Vector3>();
    public List<string> lastVoicelines = new List<string>();
    public List<int> typeOfFishEvents = new List<int>();
    public List<string> lastGuidanceSounds = new List<string>();
    public List<int> timesFishedNowhere = new List<int>();
    public List<int> typeOfWrongToolEvent = new List<int>();
    public List<bool> mapIsActive = new List<bool>();
    public List<string> activeRegion = new List<string>();
}
