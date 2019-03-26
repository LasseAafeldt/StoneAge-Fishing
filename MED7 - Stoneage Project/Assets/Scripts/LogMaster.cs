using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LogMaster : MonoBehaviour {

    public static string filePath;
    public string seperator = ",";

    public Transform player;
    public PartnerSpeech partnerSpeech;
    public SelectTool selectTool;
    //something variable where interactions are called

    void Start()
    {
        //createFile();
        //Debug.Log("I logged something");
    }
    void setPath()
    {
        filePath = Application.persistentDataPath + "/" + System.DateTime.Now.ToString() + ".txt";
    }

    //find some way to create file only once while playing
    public void createFile()
    {
        setPath();
        if (!File.Exists(filePath))
        {            
            FileStream file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            file.Close();
        }
    }

    public void logEntry(int amountOfVoicelinesPlayed, Vector3 playerPosition, int totalTorskCaught, int totalEelCaught, bool triedEelTrap, int TotalFlatfishCaught, int totalFishCaught, string attemptedInteraction, string areaTag, int timesWrongToolHasBeenSelected)
    {
        using (StreamWriter SW = new StreamWriter(File.Open(filePath, FileMode.Append, FileAccess.Write, FileShare.Write)))
        {
            Debug.Log("the file is being written in now...");
            //write the log stuff...
            //remember system.DateTime first
            string dateAndTime = System.DateTime.Now.ToString() + seperator;
            string _amountOfVoiceLinesPlayed = amountOfVoicelinesPlayed.ToString() + seperator;
            string _playerPosition = playerPosition.ToString() + seperator;
            string _TotalTorskCaught = totalTorskCaught.ToString() + seperator;
            string _totalEelCaught = totalEelCaught.ToString() + seperator;
            string _triedEeltrap = triedEelTrap.ToString() + seperator;
            string _totalFlatfishCaught = TotalFlatfishCaught.ToString() + seperator;
            string _totalFishCaught = totalFishCaught.ToString() + seperator;
            string _attemptedInteraction = attemptedInteraction.ToString() + seperator;
            string _areaTag = areaTag.ToString() + seperator;
            string _timesWrongTool = timesWrongToolHasBeenSelected.ToString();

            string[] stuffToWrite = {
                dateAndTime,
                _amountOfVoiceLinesPlayed,
                _playerPosition,
                _TotalTorskCaught,
                _totalEelCaught,
                _triedEeltrap,
                _totalFlatfishCaught,
                _totalFishCaught,
                _attemptedInteraction,
                _areaTag,
                _timesWrongTool
            };
            //string data = new string((string)dateAndTime + (string)_amountOfVoiceLinesPlayed + (string)_playerPosition + (string)_triedTorsk +(string) _triedEel + (string)_triedEeltrap + (string)_triedFlatfish + (string)_totalFishCaught + (string)_attemptedInteraction + (string)_timesWrongTool);
            
            SW.WriteLine(dateAndTime + _amountOfVoiceLinesPlayed + _playerPosition + _TotalTorskCaught + _totalEelCaught + _triedEeltrap + _totalFlatfishCaught + _totalFishCaught + _attemptedInteraction + _areaTag + _timesWrongTool);

            Debug.Log("writing in the file has been completed");            
        }
    }
}
