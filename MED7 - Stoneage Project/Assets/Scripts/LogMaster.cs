using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LogMaster : MonoBehaviour {

    public static string path;

    void Start()
    {
        createFile();
        Debug.Log("I logged something");
    }
    void setPath()
    {
        path = Application.persistentDataPath + "/" + System.DateTime.Now + ".txt";
    }

    public void createFile()
    {
        setPath();
        if (!File.Exists(path))
        {
            //File.WriteAllText(path, "I sucessfully wrote this...");
            //File.OpenWrite(path);
            FileStream file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            file.Close();
        }
    }

    public void logEntry()
    {
        FileStream file = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.None);
        //write the log stuff...
        Debug.Log("Writing something in the log file");
        file.Close();
    }
}
