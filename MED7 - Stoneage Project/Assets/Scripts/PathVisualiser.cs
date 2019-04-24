using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using System.IO;

[RequireComponent(typeof(LineRenderer))]
public class PathVisualiser : MonoBehaviour {
    #region Global Variables
    [Header("File stuff")]
    [Range(1,10)]
    public int logFileNumber;
    public string pasteDirectoryPath;

    //[SerializeField]
    private string filePath;
    private string directoryPath;


    [Header("Line stuff")]
    public Gradient gradient;
    [Range(1,5)]
    public float lineWidth;
    [HideInInspector]
    public LineRenderer ln;

    [Header("Game view camera")]
    [HideInInspector]
    public Camera camToMove;

    [HideInInspector]
    public bool hasData;

    List<Vector3> positions;
    List<Vector3> rotations;
    #endregion

    private void Start()
    {
        positions = new List<Vector3>();
        rotations = new List<Vector3>();
        camToMove = Camera.main;
        setDirectory();
    }

    public void retrieveData()
    {
        setPath();
        List<string> lines = new List<string>();
        if (File.Exists(filePath))
            lines = File.ReadAllLines(filePath).ToList();
        else
            Debug.LogError("The file you are trying to access doesn't exist");


        foreach (string line in lines)
        {
            string lin = line.Replace("(", "").Replace(")", "");
            string[] entries = lin.Split(',');
            //make vector3 and add to list
            float posX;
            float posY;
            float posZ;
            try
            {
                posX = float.Parse(entries[1]);
                posY = float.Parse(entries[2]);
                posZ = float.Parse(entries[3]);
                Vector3 newPosition = new Vector3(posX, posY, posZ);
                positions.Add(newPosition);
            }
            catch (FormatException)
            {
                //Console.WriteLine($"Unable to parse '{entries[1]}'");
                Debug.Log("Unable to parse position");
            }

            float rotX;
            float rotY;
            float rotZ;
            try
            {
                rotX = float.Parse(entries[4]);
                rotY = float.Parse(entries[5]);
                rotZ = float.Parse(entries[6]);
                Vector3 newPosition = new Vector3(rotX, rotY, rotZ);
                rotations.Add(newPosition);
            }
            catch (FormatException)
            {
                //Console.WriteLine($"Unable to parse '{entries[1]}'");
                Debug.Log("Unable to parse Rotation");
            }
        }
        if (positions.Count > 0)
            hasData = true;
        if (hasData)
            drawpath();
    }

    public void resetData()
    {
        positions.Clear();
        hasData = false;
    }

    public void setPath()
    {
        filePath = @"testWorking0" + logFileNumber.ToString() + ".txt";
    }

    public void setDirectory()
    {
        int amountOfFiles = 0;
        //directoryPath = @"C:\Users\Lasse\Desktop\ThisTest";
        directoryPath = @pasteDirectoryPath;
        string[] fileEntries = Directory.GetFiles(directoryPath);
        foreach (string entry in fileEntries)
        {
            amountOfFiles++;
        }
        Debug.Log("Files in directory: " + amountOfFiles);
    }

    void setCamera(int index)
    {
        Vector3 pos = positions[index];
        Quaternion lookAt = Quaternion.Euler(rotations[index].x, rotations[index].y, rotations[index].z);
        camToMove.transform.SetPositionAndRotation(pos, lookAt);
    }

    public void drawpath()
    {
        ln.positionCount = positions.Count;
        ln.startWidth = lineWidth;
        ln.endWidth = lineWidth;
        Vector3[] positionsArray = positions.ToArray();
        ln.SetPositions(positions.ToArray());
        ln.colorGradient = gradient;
        /*for (int i = 0; i < positions.Count; i++)
        {
            Debug.Log("Check to see if match: " + positionsArray[i]);
        }*/
    }
}
