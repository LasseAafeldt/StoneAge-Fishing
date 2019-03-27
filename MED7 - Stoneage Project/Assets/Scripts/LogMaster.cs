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
    public GameManager GM;
    public EventCatcher EC;
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

    public void logEntry(int amountOfVoicelinesPlayed, Vector3 playerPosition, int totalTorskCaught, int totalEelCaught, bool triedEelTrap, int TotalFlatfishCaught, int totalFishCaught, string latestInteraction, string areaTag, int timesFishedNowhere, int timesWrongToolHasBeenSelected)
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
            string _latestInteraction = latestInteraction.ToString() + seperator;
            string _areaTag = areaTag.ToString() + seperator;
            string _timesFishedNowhere = timesFishedNowhere.ToString() + seperator;
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
                _latestInteraction,
                _areaTag,
                _timesFishedNowhere,
                _timesWrongTool
            };
            
            SW.WriteLine(dateAndTime + _amountOfVoiceLinesPlayed + _playerPosition + _TotalTorskCaught + _totalEelCaught + _triedEeltrap + _totalFlatfishCaught + _totalFishCaught + _latestInteraction + _areaTag + _timesFishedNowhere + _timesWrongTool);
            // log entry when a sound is played...
            // should maybe log when player changes position..................................
            // log whenever a fish is caught (when total fish caught is updated)???????
            // log when eeltrap is emptied?????????
            // log whenever an interaction is made ???????????
            // log whenever the area changes ?????????
            // log whenever player fishes nowhere ??????????
            // log whenever the wrong tool is used?????????

            Debug.Log("writing in the file has been completed");            
        }
    }

    // copy paste this call where we want to call a log entry
    /*logMaster.logEntry(
                PartnerSpeech.amountOfVoiceLinesPlayed,
                logMaster.player.position,
                SelectTool.totalTorskCaught,
                SelectTool.totalEelCaught,
                SelectTool.eelTrapEmptied,
                SelectTool.totalFlatfishCaught,
                logMaster.GM.getTotalFishCaught(),
                SelectTool.latestInteraction,
                logMaster.EC.fishingArea,
                SelectTool.timesFishedNowhereTotal,
                SelectTool.amountWrongToolSelected);*/
}
