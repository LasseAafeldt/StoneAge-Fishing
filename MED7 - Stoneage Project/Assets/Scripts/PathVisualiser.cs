using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using System.IO;

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
    private string filePath;
    private string directoryPath;


    [Header("Line stuff")]
    public Gradient singleFileColor;
    public Gradient allFileColor;
    public Material lineMaterial;
    [Range(1,5)]
    public float lineWidth;

    [Header("Game view camera")]
    [HideInInspector]
    public Camera camToMove;
    [HideInInspector]
    private int camIndex;

    [HideInInspector]
    public bool hasData = false;

    List<Vector3> positions;
    List<Vector3> rotations;
    List<string> fileNames;
    List<LogFile> logFiles;
    List<LineRenderer> lineRenderers;
    GameObject lineParent;

    #endregion

    public delegate void ChangeFileToHandle(int _fileToHandle);
    public static event ChangeFileToHandle OnChangeFileToHandle;

    public delegate void ChangePositionToLookAt(int _posToLookAt);
    public static event ChangePositionToLookAt OnPosToLookAtChanged;

    //public delegate void ChangeAmountOfFilesToUse(bool _useAllDirectory);
    //public static event ChangeAmountOfFilesToUse OnAmountOfFilesToUseChanged;

    private void Start()
    {
        positions = new List<Vector3>();
        rotations = new List<Vector3>();
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
        //OnAmountOfFilesToUseChanged += drawpath;
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

    /*public bool useAllFiles
    {
        get
        {
            return useWholeDirectory;
        }

        set
        {
            if (useWholeDirectory == value)
                return;

            useWholeDirectory = value;

            if (OnAmountOfFilesToUseChanged != null)
                OnAmountOfFilesToUseChanged(useWholeDirectory);
        }
    }*/

    public void retrieveData()
    {
        resetData();
        setDirectory();
        loadAllData(logFiles);

        drawpath(logFileNumber);
    }

    public void resetData()
    {
        positions.Clear();
        rotations.Clear();
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
        foreach (LineRenderer rend in lineRenderers)
        {
            rend.positionCount = 0;
        }
    } 

    public void setDirectory()
    {
        fileNames.Clear();
        logFiles.Clear();
        int amountOfFiles = 0;
        //directoryPath = @"C:\Users\Lasse\Desktop\ThisTest";
        directoryPath = @pasteDirectoryPath;
        string[] fileEntries = Directory.GetFiles(directoryPath);
        foreach (string entry in fileEntries)
        {
            amountOfFiles++;
            fileNames.Add(entry);
            LogFile newLog = new LogFile();
            logFiles.Add(newLog);
            GameObject empty = new GameObject();
            empty.layer = LayerMask.NameToLayer("analysis");
            empty.AddComponent<LineRenderer>();
            empty.transform.SetParent(lineParent.transform);
            lineRenderers.Add(empty.GetComponent<LineRenderer>());
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
        }
    }

    void loadAllData(List<LogFile> files)
    {
        int workingFile = 0;
        Debug.Log("Starting to load all files...");
        foreach (string file in fileNames)
        {
            Debug.Log("File number: " + workingFile);
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

    public void placeGuidanceMarkers()
    {
        //place either sphere markers or gui markers at specified location.
        //give marker a code for which place is guided towards.
    }
}
