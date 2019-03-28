using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LogMaster : MonoBehaviour {

    public static string filePath;
    public string seperator = ",";

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
    #endregion

    void Start()
    {
        //fishCaught = FishCaught.None;
        //wrongTool = WrongTool.None;
    }
    private void Update()
    {
        if(filePath != null)
        {
            //do the every second call stuff here..
            InvokeRepeating("doLogEntryEverySecond", 1f, 1f);
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
                            LogMaster.WrongToolVoiceline);
                        Debug.Log("An entry is made in the log file...");
                    }
                }
            }
        }
    }

    void setPath()
    {
        //filePath = Application.persistentDataPath + "/" + System.DateTime.Now.ToString() + ".txt";
        filePath = Application.persistentDataPath + "/test.text";
        Debug.Log(filePath);
    }

    //Called in GoToLinearScene
    public void createFile()
    {
        setPath();
        /*if (!File.Exists(filePath))
        {            
            FileStream file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            file.Close();
        }*/
        if (!File.Exists(filePath))
        {
            Debug.Log("The file does not exists yet so lets create it");
            try
            {
                FileStream file = File.Create(filePath);
            }
            catch
            {
                
            }
        }
    }


    //call the function when:
    //TypeOfFishCaught is set       SelectTool script                   Done            (In SelectTools script)
    //WrongToolVoiceline is set     SelectTool script                   Done            (In SelectTools script)
    //coardboard button is pressed(so when the screen is pressed),      Done            (in this script)
    //every second if file is not already in use...                     Done            (in this script)
    public void logEntry(Vector3 _PlayerPos, Vector3 _PlayerLookRotation, string _LastVoiceline, int _TypeOfFishCaught, string _LastGuidanceSound, int _TimesFishedNowhere, int _TypeOfWrongTool)
    {
        using (StreamWriter SW = new StreamWriter(File.Open(filePath, FileMode.Append, FileAccess.Write, FileShare.Write)))
        {
            Debug.Log("the file is being written in now...");
            //write the log stuff...
            //remember system.DateTime first
            string dateAndTime = System.DateTime.Now.ToString() + seperator;
            string _playerPos = _PlayerPos.ToString() + seperator;
            string _playerLookRotation = _PlayerLookRotation.ToString() + seperator;
            string _lastVoiceline = _LastVoiceline + seperator;
            string _typeOfFishCaught = _TypeOfFishCaught.ToString() + seperator;
            string _lastGuidanceSound = _LastGuidanceSound + seperator;
            string _timesFishedNowhere = _TimesFishedNowhere.ToString() + seperator;
            string _typeOfWrongTool = _TypeOfWrongTool.ToString();

            string[] stuffToWrite = {
                dateAndTime,
                _playerPos,
                _playerLookRotation,
                _lastVoiceline,
                _typeOfFishCaught,
                _lastGuidanceSound,
                _timesFishedNowhere,
                _typeOfWrongTool
            };
            
            SW.WriteLine(dateAndTime + _playerPos + _playerLookRotation + _lastVoiceline + _typeOfFishCaught + _lastGuidanceSound + _timesFishedNowhere + _typeOfWrongTool);
            // log once a second
            //log on pointer click, so every time the button on the cardboard HMD is pressed

            Debug.Log("writing in the file has been completed");            
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
                LogMaster.WrongToolVoiceline);
            Debug.Log("An entry is made in the log file...");
        }*/

    void doLogEntryEverySecond()
    {
        if (!IsFileLocked(filePath))
        {
            logEntry(
                player.position,
                player.rotation.eulerAngles,
                PartnerSpeech.lastVoiceline,
                LogMaster.TypeOfFishCaught,
                GuidanceSounds.lastGuidanceSound,
                SelectTool.timesFishedNowhereTotal,
                LogMaster.WrongToolVoiceline);
            Debug.Log("An entry is made in the log file...");
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
