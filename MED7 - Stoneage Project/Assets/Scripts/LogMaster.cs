using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class LogMaster : MonoBehaviour {

    public static bool shouldBeLogging = true;

    public static string filePath;
    public static string directoryPath;
    public string seperator = ",";
    int timesIHaveLogged = 0;

    #region Log Planning
    //Log entry should be called when TypeOfFishCaught is set,
    //when WrongToolVoiceline is set,
    //When coardboard button is pressed (so when the screen is pressed),
    //every second if file is not already in use...

    //log data is gotten from these...
    public Transform player;
    //call PartnerSpeech.lastVoiceline to get latest voiceline
    public static int TypeOfFishCaught = 0; //Set in SelectTool
    //call GuidanceSounds.latestGuideSound to get last guidance sound
    //call SelectTool.timesFishedNowhereTotal to get total times fished nowhere
    public static int WrongToolVoiceline = 0; //set in SelectTool
    public bool mapOnCamera;
    #endregion

    static int logFileNumber = 0;
    void Start()
    {
        timesIHaveLogged = 0;
        if (filePath != null)
        {
            //Debug.Log("I should have a file path now...");
            //do the every second call stuff here..
            if(shouldBeLogging)
                InvokeRepeating("doLogEntryEverySecond", 0f, 1f);
        }
    }
    private void Update()
    {
        if(filePath != null)
        {
            if(Input.touchCount > 0) //if the screen is touched this frame
            {
                if(Input.GetTouch(0).phase == TouchPhase.Began) //if the screen has just been pressed this frame
                {
                    if (IsFileLocked(filePath))
                    {
                        //returns out of the function if the file is already in use...
                        return;
                    }
                    //do a log entry
                    if (LogMaster.filePath != null)
                    {
                        logEntry(
                            player.position,
                            player.rotation.eulerAngles,
                            PartnerSpeech.lastVoiceline,
                            LogMaster.TypeOfFishCaught,
                            GuidanceSounds.lastGuidanceSound,
                            SelectTool.timesFishedNowhereTotal,
                            LogMaster.WrongToolVoiceline,
                            mapOnCamera = false,
                            BoatControllerScript.currentlyInRegion);

                        Debug.Log("An entry is made in the log file...");
                    }
                }
            }
        }
        //reset the two enums so they are only logged when they actually happen...
        TypeOfFishCaught = 0;
        WrongToolVoiceline = 0;
    }

    void setPath()
    {
        logFileNumber++;
        //Debug.Log("number of files that should exist: " + logFileNumber);
        //filePath = Application.persistentDataPath + "/" + System.DateTime.Now.ToString() + ".txt";
        string date = System.DateTime.Today.ToShortDateString();
        string timeNow = System.DateTime.UtcNow.ToLongTimeString();

        directoryPath = Application.persistentDataPath + date;
        /*if(!GameManager.singleton.debugLogging)
            filePath = directoryPath + timeNow + ".txt";
        else
        {
            filePath = "testWorking0" + logFileNumber.ToString() + ".txt";
        }*/
        filePath = "testWorking0" + logFileNumber.ToString() + ".txt";
    }

    //Called in GoToLinearScene
    public void createFile()
    {
        setPath();
        FileInfo fileInfo = new FileInfo(directoryPath);
        fileInfo.Directory.Create();
        //fileInfo.Directory.CreateSubdirectory("/test01");
        if (!File.Exists(filePath))
        {
            Debug.Log("The file does not exists yet so lets create it");
            try
            {
                FileStream file = File.Create(filePath);
                file.Close();
            }
            catch(System.Exception e)
            {
                Debug.LogError(e);
                // do something to show the error
            }
        }
    }


    //call the function when:
    //TypeOfFishCaught is set       SelectTool script                   Done            (In SelectTools script)
    //WrongToolVoiceline is set     SelectTool script                   Done            (In SelectTools script)
    //map is setActive              SelectTool script                   Done            (In SelectTools script)
    //coardboard button is pressed(so when the screen is pressed),      Done            (in this script)
    //every second if file is not already in use...                     Done            (in this script)
    public void logEntry(Vector3 _PlayerPos, Vector3 _PlayerLookRotation, string _LastVoiceline, int _TypeOfFishCaught, string _LastGuidanceSound, int _TimesFishedNowhere, int _TypeOfWrongTool, bool _MapActive, string activeRegion)
    {
        if(filePath == null)
        {
            return;
        }
        //Debug.Log("the file is being written in now...");
        //write the log stuff...
        //remember system.DateTime first
        string dateAndTime = System.DateTime.Now.ToString() + seperator;
        string _playerPos = _PlayerPos.ToString() + seperator;
        string _playerLookRotation = _PlayerLookRotation.ToString() + seperator;
        string _lastVoiceline = _LastVoiceline + seperator;
        string _typeOfFishCaught = _TypeOfFishCaught.ToString() + seperator;
        string _lastGuidanceSound = _LastGuidanceSound + seperator;
        string _timesFishedNowhere = _TimesFishedNowhere.ToString() + seperator;
        string _typeOfWrongTool = _TypeOfWrongTool.ToString() + seperator;
        string _mapActive = _MapActive.ToString() + seperator;
        string _activeRegion = activeRegion;

        string[] stuffToWrite = {
            dateAndTime,
            _playerPos,
            _playerLookRotation,
            _lastVoiceline,
            _typeOfFishCaught,
            _lastGuidanceSound,
            _timesFishedNowhere,
            _typeOfWrongTool,
            _mapActive,
            _activeRegion
        };
        using (StreamWriter SW = new StreamWriter(File.Open(filePath, FileMode.Append, FileAccess.Write, FileShare.Write)))
        {
            
            SW.WriteLine(dateAndTime + _playerPos + _playerLookRotation + _lastVoiceline + _typeOfFishCaught + _lastGuidanceSound + _timesFishedNowhere + _typeOfWrongTool + _mapActive + _activeRegion + seperator + timesIHaveLogged.ToString());
            timesIHaveLogged++;
            //Debug.Log("writing in the file has been completed");            
        }
    }

    // copy paste this call where we want to call a log entry
    /*if(LogMaster.filePath != null)
        {
            logMaster.logEntry(
                logMaster.player.position,
                logMaster.player.rotation.eulerAngles,
                PartnerSpeech.lastVoiceline,
                LogMaster.TypeOfFishCaught,
                GuidanceSounds.lastGuidanceSound,
                SelectTool.timesFishedNowhereTotal,
                LogMaster.WrongToolVoiceline,
                logmaster.mapOnCamera.activeSelf,
                BoatControllerScript.currentlyInRegion);
            Debug.Log("An entry is made in the log file...");
        }*/

    void doLogEntryEverySecond()
    {
        //Debug.Log("Do log entry every second has been called");
        //Debug.LogWarning("Log will break in endscene if that scene is not named: End Scene");
        if (!IsFileLocked(filePath))
        {
            if (!shouldBeLogging)
            {
                CancelInvoke();
                return;
            }
            logEntry(
                player.position,
                player.rotation.eulerAngles,
                PartnerSpeech.lastVoiceline,
                TypeOfFishCaught,
                GuidanceSounds.lastGuidanceSound,
                SelectTool.timesFishedNowhereTotal,
                WrongToolVoiceline,
                mapOnCamera = false,
                BoatControllerScript.currentlyInRegion);
            //Debug.Log("An entry is made in the log file...");
        }
    }

    public bool IsFileLocked(string filename)
    {
        bool Locked = false;
        try
        {
            FileStream fs =
                File.Open(filename, FileMode.OpenOrCreate,
                FileAccess.ReadWrite, FileShare.None);
            fs.Close();
        }
        catch (IOException ex)
        {
            Locked = true;
        }
        return Locked;
    }
}
