using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEditor;

/*public class ReadOnlyAttribute : PropertyAttribute
{

}

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property,
                                            GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position,
                               SerializedProperty property,
                               GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}*/

public class PathVisualiser : MonoBehaviour {
    #region Global Variables
    //[Header("Toggle single file or all files")]
    [HideInInspector]
    public bool useWholeDirectory;

    [Header("File stuff")]
    public string pasteDirectoryPath;
    [HideInInspector]
    private int logFileNumber;
    [HideInInspector]
    public int numberOfFilesInDirectory;

    //[SerializeField]
    //[ReadOnly]
    public string filePath;
    private string directoryPath;


    [Header("Line stuff")]
    public Gradient singleFileColor;
    public Gradient allFileColor;
    public Material lineMaterial;
    [Range(1,5)]
    public float lineWidth;

    [Header("Guidance indicators")]
    public GameObject IndicatorObject;
    //public GameObject mapActivatedIndicator;
    [ColorUsage(false)]
    public Color guideToTorsk;
    [ColorUsage(false)]
    public Color guideToEel;
    [ColorUsage(false)]
    public Color guideToEeltrap;
    [ColorUsage(false)]
    public Color guideToFlatfish;
    [ColorUsage(false)]
    public Color mapActiveColor;
    [ColorUsage(false)]
    public Color mapDeactiveColor;

    [SerializeField]
    private Material indiMatTorsk;
    [SerializeField]
    private Material indiMatEel;
    [SerializeField]
    private Material indiMatEeltrap;
    [SerializeField]
    private Material indiMatFlatfish;
    [SerializeField]
    private Material indiMatMap;
    [SerializeField]
    private Material indiMatMapDeactive;

    [Header("Fish caught indicators")]
    public GameObject FishIndicatorObject;
    [ColorUsage(false)]
    public Color Fish1Color;
    [ColorUsage(false)]
    public Color Fish2Color;
    [ColorUsage(false)]
    public Color Fish3Color;
    [SerializeField]
    private Material fish1Mat;
    [SerializeField]
    private Material fish2Mat;
    [SerializeField]
    private Material fish3Mat;

    [Header("Game view camera")]
    [HideInInspector]
    public Camera camToMove;
    [HideInInspector]
    private int camIndex;

    [HideInInspector]
    public bool hasData = false;

    //List<Vector3> positions;
    //List<Vector3> rotations;
    List<string> fileNames;
    List<LogFile> logFiles;
    List<LineRenderer> lineRenderers;
    GameObject lineParent;

    #endregion

    public delegate void ChangeFileToHandle(int _fileToHandle);
    public static event ChangeFileToHandle OnChangeFileToHandle;

    public delegate void ChangePositionToLookAt(int _posToLookAt);
    public static event ChangePositionToLookAt OnPosToLookAtChanged;

    private void Start()
    {
        //positions = new List<Vector3>();
        //rotations = new List<Vector3>();
        fileNames = new List<string>();
        logFiles = new List<LogFile>();
        lineRenderers = new List<LineRenderer>();
        lineParent = GameObject.Find("lineParent");
        hasData = false;
        camToMove = Camera.main;
        camIndex = 0;
        //setDirectory();
        OnChangeFileToHandle += drawpath;
        OnPosToLookAtChanged += setCamera;
    }


    public int fileToSelect
    {
        get
        {
            return logFileNumber;
        }

        set
        {
            if (logFileNumber == value)
                return;

            logFileNumber = value;
            filePath = fileNames[logFileNumber];

            if (OnChangeFileToHandle != null)
                OnChangeFileToHandle(logFileNumber);
        }
    }

    public int positionToLookAt
    {
        get
        {
            return camIndex;
        }

        set
        {
            if (camIndex == value)
                return;

            camIndex = value;

            if (OnPosToLookAtChanged != null)
                OnPosToLookAtChanged(camIndex);
        }
    }

    public void retrieveData()
    {
        resetData();
        setDirectory();
        loadAllData(logFiles);

        drawpath(logFileNumber);
    }

    public void resetData()
    {
        //positions.Clear();
        //rotations.Clear();
        fileNames.Clear();
        logFiles.Clear();
        foreach (LineRenderer rend in lineRenderers)
        {
            rend.positionCount = 0;
        }
        lineRenderers.Clear();
        for (int i = 0; i < lineParent.transform.childCount; i++)
        {
            GameObject child = lineParent.transform.GetChild(i).gameObject;
            Destroy(child);
        }
        hasData = false;
        numberOfFilesInDirectory = 2;
    }

    private void resetPathDrawn()
    {
        lineRenderers.Clear();
        for (int i = 0; i < lineParent.transform.childCount; i++)
        {
            GameObject child = lineParent.transform.GetChild(i).gameObject;
            Destroy(child);
        }
    } 

    public void setDirectory()
    {
        List<string> fileList = new List<string>();
        List<string> subFolders = new List<string>();
        fileNames.Clear();
        logFiles.Clear();
        fileList.Clear();
        subFolders.Clear();
        int amountOfFiles = 0;
        //directoryPath = @"C:\Users\Lasse\Desktop\ThisTest";
        directoryPath = @pasteDirectoryPath;

        string[] subdirectories = Directory.GetDirectories(directoryPath);
        foreach (string folder in subdirectories)
        {
            subFolders.Add(folder);
            string[] subDir = Directory.GetDirectories(folder);
            foreach (string subFold in subDir)
            {
                subFolders.Add(subFold);
            }
        }

        foreach (string subfolder in subFolders)
        {
            string[] fileEntries = Directory.GetFiles(subfolder);
            foreach (string file in fileEntries)
            {
                fileList.Add(file);
            }
        }
        foreach (string entry in fileList)
        {
            amountOfFiles++;
            fileNames.Add(entry);
            LogFile newLog = new LogFile();
            logFiles.Add(newLog);
            
        }
        Debug.Log("Files in directory: " + amountOfFiles);
        numberOfFilesInDirectory = amountOfFiles;
    }

    void setCamera(int index)
    {
        if (logFiles[logFileNumber] == null || !Application.isPlaying)
            return;
        Debug.Log("I am starting to set camera!");
        LogFile file = logFiles[logFileNumber];
        Vector3 pos = file.positions[index];
        Debug.Log("I have assigned the desired position to local varible");
        Quaternion lookAt = Quaternion.Euler(file.rotations[index].x, file.rotations[index].y, file.rotations[index].z);
        Debug.Log("I have created a quaternion that should hopefully work");
        camToMove.transform.SetPositionAndRotation(pos, lookAt);
        Debug.Log("I have called cam set position and rotation");
    }

    public void drawpath(int fileNumber)
    {
        if (!hasData)
            return;
        resetPathDrawn();
        //string[] fileEntries = Directory.GetFiles(directoryPath);
        foreach (string entry in fileNames)
        {
            GameObject empty = new GameObject();
            empty.layer = LayerMask.NameToLayer("analysis");
            empty.AddComponent<LineRenderer>();
            empty.transform.SetParent(lineParent.transform);
            lineRenderers.Add(empty.GetComponent<LineRenderer>());
        }
        Debug.Log("Starting to draw path...");
        //do stuff that are same for all LineRenderes here
        foreach (LineRenderer rend in lineRenderers)
        {
            rend.startWidth = lineWidth;
            rend.endWidth = lineWidth;
            rend.material = lineMaterial;
            rend.sortingOrder = 5;
        }
        if (useWholeDirectory)
        {
            int workingFile = 0;
            foreach (LogFile file in logFiles)
            {
                LineRenderer rend = lineRenderers[workingFile];

                rend.positionCount = file.positions.Count;
                rend.SetPositions(file.positions.ToArray());
                //rend.startColor = lineColor;
                //rend.endColor = lineColor;
                rend.colorGradient = allFileColor;

                workingFile++;
            }
            if (lineRenderers[0].positionCount > 0)
                hasData = true;
        }
        else
        {
            LineRenderer rend = lineRenderers[fileNumber];
            rend.positionCount = logFiles[fileNumber].positions.Count;
            if (logFiles[fileNumber].positions.Count > 0)
            {
                hasData = true;
                rend.SetPositions(logFiles[fileNumber].positions.ToArray());
                rend.colorGradient = singleFileColor;
            }
            placeGuidanceMarkers(fileNumber);
            placeFishingMarkers(fileNumber);
        }
    }

    void loadAllData(List<LogFile> files)
    {
        int workingFile = 0;
        Debug.Log("Starting to load all files...");
        foreach (string file in fileNames)
        {
            Debug.Log("File Index: " + workingFile);
            List<string> lines = new List<string>();
            if (File.Exists(file))
                lines = File.ReadAllLines(file).ToList();
            else
            {
                Debug.LogError("File number: "+workingFile+" doesn't exist. File path: "+file);
                continue;
            }
            foreach (string line in lines)
            {
                string lin = line.Replace("(", "").Replace(")", "");
                string[] entries = lin.Split(',');

                // read  and store all the log data
                #region read and store position data
                float posX;
                float posY;
                float posZ;
                try
                {
                    posX = float.Parse(entries[1]);
                    posY = float.Parse(entries[2]);
                    posZ = float.Parse(entries[3]);
                    Vector3 newPosition = new Vector3(posX, posY, posZ);
                    //positions.Add(newPosition);
                    files[workingFile].positions.Add(newPosition);
                }
                catch (FormatException)
                {
                    //Console.WriteLine($"Unable to parse '{entries[1]}'");
                    Debug.Log("Unable to parse position");
                }
                #endregion

                #region read and store camera rotation data
                float rotX;
                float rotY;
                float rotZ;
                try
                {
                    rotX = float.Parse(entries[4]);
                    rotY = float.Parse(entries[5]);
                    rotZ = float.Parse(entries[6]);
                    Vector3 newRotation = new Vector3(rotX, rotY, rotZ);
                    files[workingFile].rotations.Add(newRotation);
                }
                catch (FormatException)
                {
                    //Console.WriteLine($"Unable to parse '{entries[1]}'");
                    Debug.Log("Unable to parse Rotation");
                }
                #endregion

                #region read and store last played voicline
                string voiceline = entries[7];
                files[workingFile].lastVoicelines.Add(voiceline);
                #endregion

                #region read and store type of fish event
                int eventType;
                try
                {
                    eventType = int.Parse(entries[8]);
                }
                catch
                {
                    Debug.Log("Unable to parse type of fish event");
                    eventType = 0;
                }
                files[workingFile].typeOfFishEvents.Add(eventType);
                #endregion

                #region read and store last guidance sound
                string lastGuidance = entries[9];
                files[workingFile].lastGuidanceSounds.Add(lastGuidance);
                #endregion

                #region read and store times fished nowhere
                int timesFishNowhere;
                try
                {
                    timesFishNowhere = int.Parse(entries[10]);
                }
                catch
                {
                    Debug.Log("Unable to parse times fished nowhere");
                    timesFishNowhere = 0;
                }
                files[workingFile].timesFishedNowhere.Add(timesFishNowhere);
                #endregion

                #region read and store type of wrong tool event
                int wrongToolEvent;
                try
                {
                    wrongToolEvent = int.Parse(entries[11]);
                }
                catch
                {
                    Debug.Log("Unable to parse wrong tool event");
                    wrongToolEvent = 0;
                }
                files[workingFile].typeOfWrongToolEvent.Add(wrongToolEvent);
                #endregion

                #region read and store map is active
                bool mapIsActive;
                try
                {
                    mapIsActive = bool.Parse(entries[12]);
                }
                catch
                {
                    Debug.Log("Unable to parse map is active");
                    mapIsActive = false;
                }
                files[workingFile].mapIsActive.Add(mapIsActive);
                #endregion

                #region read and store acive region
                string activeRegion = entries[13];
                files[workingFile].activeRegion.Add(activeRegion);
                #endregion
            }
            workingFile++;
        }
        if (files[0].positions.Count > 0)
            hasData = true;
        Debug.Log("Finished loading the data from all files");
    }

    public int getMaxEntriesInFile(int fileNumber)
    {
        int entries;
        if (logFiles[fileNumber] != null)
        {
            entries = logFiles[fileNumber].positions.Count;
        }
        else
        {
            entries = 2;
        }
        return entries;
    }

    public void placeGuidanceMarkers(int file)
    {
        //make sound makers
        #region Sound markers
        List<string> _lastGuidanceSound = logFiles[file].lastGuidanceSounds;
        List<Vector3> positions = logFiles[file].positions;
        //find positions of where a new guidance sound is played
        List<int> soundPosistions = new List<int>();
        string tempSound = "";
        int workingPosition = 0;
        foreach (string sound in _lastGuidanceSound)
        {
            if (!sound.Equals(tempSound))
            {
                //Debug.Log("A new guidance sound was discovered");
                soundPosistions.Add(workingPosition);
            }
            tempSound = sound;
            workingPosition++;
        }
        //draw indication
        //give marker a code for which place is guided towards.
        foreach (int position in soundPosistions)
        {
            Vector3 newpos = new Vector3(positions[position].x, 3, positions[position].z);
            GameObject indicator = GameObject.Instantiate(IndicatorObject, newpos, Quaternion.identity, lineParent.transform);
            Renderer rend = indicator.GetComponent<Renderer>();
            //rend.sharedMaterial = indicationMaterial;
            if (_lastGuidanceSound[position].Contains("torsk") || _lastGuidanceSound[position].Contains("Torsk "))
            {
                rend.sharedMaterial = indiMatTorsk;
                rend.sharedMaterial.color = guideToTorsk;
                //Debug.Log("Sound: " + _lastGuidanceSound[position]);
            }
            if (_lastGuidanceSound[position].Equals("Guide mod ål") || _lastGuidanceSound[position].Contains("Ål "))
            {
                rend.sharedMaterial = indiMatEel;
                rend.sharedMaterial.color = guideToEel;
                //Debug.Log("Sound: " + _lastGuidanceSound[position]);
            }
            if (_lastGuidanceSound[position].Contains("Guide mod åleruse"))
            {
                rend.sharedMaterial = indiMatEeltrap;
                rend.sharedMaterial.color = guideToEeltrap;
                //Debug.Log("Sound: " + _lastGuidanceSound[position]);
            }
            if (_lastGuidanceSound[position].Contains("guide mod r¢dspætter") || _lastGuidanceSound[position].Contains("r¢dspætte "))
            {
                rend.sharedMaterial = indiMatFlatfish;
                rend.sharedMaterial.color = guideToFlatfish;
                //Debug.Log("Sound: " + _lastGuidanceSound[position]);
            }
        }
        #endregion

        //make map markers
        List<bool> _mapActivated = logFiles[file].mapIsActive;
        List<int> mapActivedPositions = new List<int>();
        List<int> mapDeactivatedPos = new List<int>();
        bool tempMap = false;
        int workingPositionMap = 0;
        //determine the positions where the map becomes active
        foreach (bool map in _mapActivated)
        {
            //Debug.Log("checking each mapIsActive Log in file");
            //if the value is different from the previous value
            if (!map.Equals(tempMap))
            {
                Debug.Log("A change in map status occured. Map = " + map);
                //if this change is to true then add the position to the list
                if(map.Equals(true))
                {
                    mapActivedPositions.Add(workingPositionMap);
                }
                if(map.Equals(false))
                {
                    mapDeactivatedPos.Add(workingPositionMap);
                }
            }
            tempMap = map;
            workingPositionMap++;
        }
        //instatiate marker for map
        //Debug.Log("I fisnished loading in all the times map was activated");
        foreach (int position in mapActivedPositions)
        {
            Vector3 newpos = new Vector3(positions[position].x, 3, positions[position].z);
            GameObject indicator = GameObject.Instantiate(IndicatorObject, newpos, Quaternion.identity, lineParent.transform);
            Renderer rend = indicator.GetComponent<Renderer>();
            rend.sharedMaterial = indiMatMap;
            rend.sharedMaterial.color = mapActiveColor;
        }
        foreach (int position in mapDeactivatedPos)
        {
            float offsetDistance = 10f;
            Vector3 travelDir = positions[position+1] - positions[position];
            Vector2 perpen = Vector2.Perpendicular(new Vector2(travelDir.x, travelDir.z));
            Vector3 offset = new Vector3(perpen.x, travelDir.y, perpen.y);
            Vector3 newpos = new Vector3(positions[position].x, 3, positions[position].z);
            GameObject indicator = GameObject.Instantiate(IndicatorObject, newpos, Quaternion.identity, lineParent.transform);
            indicator.transform.Translate(offset.normalized * offsetDistance, Space.Self);
            Renderer rend = indicator.GetComponent<Renderer>();
            rend.sharedMaterial = indiMatMapDeactive;
            rend.sharedMaterial.color = mapDeactiveColor;
        }
    }

    public void placeFishingMarkers(int file)
    {
        //get the different fish events
        List<int> fishEvents = logFiles[file].typeOfFishEvents;
        List<Vector3> positions = logFiles[file].positions;
        List<int> caughtTorsk = new List<int>();
        List<int> caughtEel = new List<int>();
        List<int> caughtFlatfish = new List<int>();
        List<int> caughtEeltrap = new List<int>();
        int lineNumber = 1;
        foreach (int _event in fishEvents)
        {
            if (_event == 1)
                caughtTorsk.Add(lineNumber);
            if (_event == 2)
                caughtEel.Add(lineNumber);
            if (_event == 3)
                caughtFlatfish.Add(lineNumber);
            if (_event == 4)
                caughtEeltrap.Add(lineNumber);

            lineNumber++;
        }
        Debug.Log("fish caught: Torsk = " + caughtTorsk.Count + " Eel = " + caughtEel.Count + " Flatfish = " + caughtFlatfish.Count);
        //Draw indicators for fish caught
        int workingFish = 1;

        foreach (int fish in caughtTorsk)
        {
            Debug.Log("position Torsk: "+ fish);
            Debug.Log("length: " + caughtTorsk.Count);
            Vector3 newpos = new Vector3(positions[fish].x, 3, positions[fish].z);
            GameObject indicator = GameObject.Instantiate(FishIndicatorObject, newpos, Quaternion.identity, lineParent.transform);
            Renderer rend = indicator.GetComponent<Renderer>();
            if(workingFish == 1)
            {
                rend.sharedMaterial = fish1Mat;
                rend.sharedMaterial.color = Fish1Color;
            }
            if (workingFish == 2)
            {
                rend.sharedMaterial = fish2Mat;
                rend.sharedMaterial.color = Fish2Color;
            }
            if (workingFish == 3)
            {
                rend.sharedMaterial = fish3Mat;
                rend.sharedMaterial.color = Fish3Color;
            }
            workingFish++;
        }
        workingFish = 1;
        foreach (int fish in caughtEel)
        {
            Debug.Log("position eel: " + fish);
            Debug.Log("length: " + caughtEel.Count);
            Vector3 newpos = new Vector3(positions[fish].x, 3, positions[fish].z);
            GameObject indicator = GameObject.Instantiate(FishIndicatorObject, newpos, Quaternion.identity, lineParent.transform);
            Renderer rend = indicator.GetComponent<Renderer>();
            if (workingFish == 1)
            {
                rend.sharedMaterial = fish1Mat;
                rend.sharedMaterial.color = Fish1Color;
            }
            if (workingFish == 2)
            {
                rend.sharedMaterial = fish2Mat;
                rend.sharedMaterial.color = Fish2Color;
            }
            if (workingFish == 3)
            {
                rend.sharedMaterial = fish3Mat;
                rend.sharedMaterial.color = Fish3Color;
            }
            workingFish++;
        }
        workingFish = 1;
        foreach (int fish in caughtFlatfish)
        {
            Debug.Log("position flatfish: " + fish);
            Debug.Log("length: " + caughtFlatfish.Count);
            Vector3 newpos = new Vector3(positions[fish].x, 3, positions[fish].z);
            GameObject indicator = GameObject.Instantiate(FishIndicatorObject, newpos, Quaternion.identity, lineParent.transform);
            Renderer rend = indicator.GetComponent<Renderer>();
            if (workingFish == 1)
            {
                rend.sharedMaterial = fish1Mat;
                rend.sharedMaterial.color = Fish1Color;
            }
            if (workingFish == 2)
            {
                rend.sharedMaterial = fish2Mat;
                rend.sharedMaterial.color = Fish2Color;
            }
            if (workingFish == 3)
            {
                rend.sharedMaterial = fish3Mat;
                rend.sharedMaterial.color = Fish3Color;
            }
            workingFish++;
        }
        workingFish = 1;
        foreach (int fish in caughtEeltrap)
        {
            Debug.Log("position eeltrap: " + fish);
            Debug.Log("length: " + caughtEeltrap.Count);
            Vector3 newpos = new Vector3(positions[fish].x, 3, positions[fish].z);
            GameObject indicator = GameObject.Instantiate(FishIndicatorObject, newpos, Quaternion.identity, lineParent.transform);
            Renderer rend = indicator.GetComponent<Renderer>();
            if (workingFish == 1)
            {
                rend.sharedMaterial = fish1Mat;
                rend.sharedMaterial.color = Fish1Color;
            }
            if (workingFish == 2)
            {
                rend.sharedMaterial = fish2Mat;
                rend.sharedMaterial.color = Fish2Color;
            }
            if (workingFish == 3)
            {
                rend.sharedMaterial = fish3Mat;
                rend.sharedMaterial.color = Fish3Color;
            }
            workingFish++;
        }

    }
}
