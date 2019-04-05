using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTool : MonoBehaviour {

public bool IsTribeBasket;
    enum FishCaught //may need to map this to a static int....
    {
        None,
        Torsk,
        Eel,
        Flatfish,
        EeltrapEmptied
    }
    [HideInInspector]
    FishCaught fishCaught; //call with the right fish in select tool
    public enum WrongTool
    {
        None,
        NoIron4Cod,
        NoHook4Eel
    }
    [HideInInspector]
    public WrongTool wrongTool; //call with the wright wrong tool voiceline in select tool

    public static bool eelTrapEmptied = false;    
    public static int timesFishedNowhereTotal = 0;

    private static int timesFishedNoWhere;

EventCatcher EC;
PartnerAnimator PA;
    PartnerSpeech partnerSpeech;
    GuidanceSounds guidance;
    LogMaster logMaster;

	string tool ="";

    private bool endGame = false;
    private bool firstFishingTorsk = true;
    private bool secondFishingTorsk = false;
    private bool thirdFishingTorsk = false;
    private bool firstFishingEel = true;
    private bool secondFishingEel = false;
    private bool thirdFishingEel = false;
    private bool firstFishingFlatfish = true;
    private bool secondFishingFlatfish = false;
    private bool thirdFishingFlatfish = false;
    // Use this for initialization
    void Start () {
		EC = GameManager.singleton.boat.GetComponent<EventCatcher>();
		try
		{
			PA = GameManager.singleton.partner.GetComponent<PartnerAnimator>();
            partnerSpeech = GameManager.singleton.partner.GetComponent<PartnerSpeech>();
            guidance = GameManager.singleton.CameraContainer.GetComponent<GuidanceSounds>();
            logMaster = GameManager.singleton.logMaster;

        }
		catch{}
        timesFishedNoWhere = 0;
    }
	
	public void Select(){
		//selecting which tool to use 
		if(tag == "hook")
		{
			Debug.Log("selected hook");
			tool = tag;
			//play animation

			if (GameManager.singleton.boat.GetComponent<EventCatcher>().fishingArea == "TorskArea")
			{
				HideTool();
				PA.HookAnimation();
                Debug.Log("Player is fishing torsk now, hopefully the sound is playing while the animation happens");
                if (firstFishingTorsk)
                {
                    partnerSpeech.PartnerSaysSomething(partnerSpeech.whileTorskFishingFirst);
                    firstFishingTorsk = false;
                    secondFishingTorsk = true;
                    EC.startFishing(tool);
                    return;
                }
                if (secondFishingTorsk)
                {
                    partnerSpeech.PartnerSaysSomething(partnerSpeech.whileTorskFishingSecond);
                    secondFishingTorsk = false;
                    thirdFishingTorsk = true;
                    EC.startFishing(tool);
                    return;
                }
                if (thirdFishingTorsk)
                {
                    partnerSpeech.PartnerSaysSomething(partnerSpeech.whileTorskFishingThird);
                    thirdFishingTorsk = false;
                    EC.startFishing(tool);
                    return;
                }
                Debug.Log("player has fished torsk 3 times now");
			}
			else if(GameManager.singleton.boat.GetComponent<EventCatcher>().fishingArea == "")
			{
                //no fish here
                notEnoughFishHere();
			}
			else
			{
                //wrong tool
                setWrongToolVoicelineInt(WrongTool.NoIron4Cod);
                Debug.Log("this is not the tool to use for torsk");
				GameManager.singleton.partner.GetComponent<PartnerSpeech>().PartnerSaysSomething(
					GameManager.singleton.partner.GetComponent<PartnerSpeech>().NoHook4CodTryIron);
			}
		}
		else if (tag == "eeliron")
		{
			Debug.Log("selected eeliron");
			tool = tag;
			//play animation

			if (GameManager.singleton.boat.GetComponent<EventCatcher>().fishingArea == "EelArea")
			{
				HideTool();
				PA.EelironAnimation();
                Debug.Log("Player is fishing eel now, hopefully the sound is playing while the animation happens");
                if (firstFishingEel)
                {
                    partnerSpeech.PartnerSaysSomething(partnerSpeech.whileEelFishingFirst);
                    firstFishingEel = false;
                    secondFishingEel = true;
                    EC.startFishing(tool);
                    return;
                }
                if (secondFishingEel)
                {
                    partnerSpeech.PartnerSaysSomething(partnerSpeech.whileEelFishingSecond);
                    secondFishingEel = false;
                    thirdFishingEel = true;
                    EC.startFishing(tool);
                    return;
                }
                if (thirdFishingEel)
                {
                    partnerSpeech.PartnerSaysSomething(partnerSpeech.whileEelFishingThird);
                    thirdFishingEel = false;
                    EC.startFishing(tool);
                    return;
                }
                Debug.Log("player has fished eel 3 times now");
            }
            else if(GameManager.singleton.boat.GetComponent<EventCatcher>().fishingArea == "FlatfishArea")
            {
                HideTool();
                PA.EelironAnimation();
                Debug.Log("Player is fishing Flatfish now, hopefully the sound is playing while the animation happens");
                if (firstFishingFlatfish)
                {
                    partnerSpeech.PartnerSaysSomething(partnerSpeech.whileFlatfishFihingFirst);
                    firstFishingFlatfish = false;
                    secondFishingFlatfish = true;
                    EC.startFishing(tool);
                    return;
                }
                if (secondFishingFlatfish)
                {
                    partnerSpeech.PartnerSaysSomething(partnerSpeech.whileFlatfishFihingSecond);
                    secondFishingFlatfish = false;
                    thirdFishingFlatfish = true;
                    EC.startFishing(tool);
                    return;
                }
                if (thirdFishingFlatfish)
                {
                    partnerSpeech.PartnerSaysSomething(partnerSpeech.whileFlatfishFihingThird);
                    thirdFishingFlatfish = false;
                    EC.startFishing(tool);
                    return;
                }
                Debug.Log("player has fished Flatfish 3 times now");
            }
			else if(GameManager.singleton.boat.GetComponent<EventCatcher>().fishingArea == "")
			{
                //no fish here
                notEnoughFishHere();
			}
			else
			{
                //wrong tool
                setWrongToolVoicelineInt(WrongTool.NoHook4Eel);

                Debug.Log("This is not the tool to use for eel or Flatfish");
                GameManager.singleton.partner.GetComponent<PartnerSpeech>().PartnerSaysSomething(
					GameManager.singleton.partner.GetComponent<PartnerSpeech>().NoIron4CodTryHook);
			}			
		}
        else if (tag == "map")
        {
            if (GameManager.singleton.map.activeSelf == true)
            {
                GameManager.singleton.map.SetActive(false);
                GameManager.singleton.mapOnCam.SetActive(true);
            }
            else
            {
                GameManager.singleton.map.SetActive(true);
                GameManager.singleton.mapOnCam.SetActive(false);
            }
            Debug.Log("selected map");
            tool = tag;
            //play animation
        }

        setWrongToolVoicelineInt(WrongTool.None);
        //admit to fish with the selected tool
        //EC.startFishing(tool);
    }

    #region empty basket 
    //PA.BasketAnimation is apparently what empties the basket and adds fish to the basket, or well it makes it somehow
        // transition to a function in PartnerAnimator called BasketAniDone() which takes care of adding and instanciating the fish.
    public void EmptyBasket()
	{
		if(tag =="emptyBasket")
		{
            GameManager.singleton.canMove = false;
            //Debug.Log("I can't move now");
			PA.BasketAnimation();
            Debug.Log("empty eeltrap animation has just been called so lets play the saound aswell");
            partnerSpeech.PartnerSaysSomething(partnerSpeech.whileEmptyingEeltrap);

            eelTrapEmptied = true;
			//Debug.Log("emptying basket for test");
			GetComponent<Collider>().enabled=false;			
		}
	}

    public void EndGame()
    {
        if (tag == "EndGame")
        {
            if (endGame)
            {
                Debug.Log("Player is moving on to end scene");
                partnerSpeech.PartnerSaysSomething(partnerSpeech.endIsConfirmed);
                //waiting to load end scene till the audioclip is done playing
                StartCoroutine(waitforAudioToLoadEndScene(partnerSpeech.endIsConfirmed));
                //EC.CheckForEnding();
                return;
            }
            Debug.Log("player is attempting to end game");
            //play voiceline "are you sure you want to end"
            partnerSpeech.PartnerSaysSomething(partnerSpeech.confirmEndPlz);
            endGame = true;

        }
    }

    public void notEnoughFishHere()
    {
        timesFishedNowhereTotal++;
        if(timesFishedNoWhere >= partnerSpeech.noFishHere.Length)
        {
            guidance.playGuidanceSound();
            timesFishedNoWhere = 0;
            return;
        }
        if(timesFishedNoWhere < partnerSpeech.noFishHere.Length)
        {
            Debug.Log("no fish here array: " + timesFishedNoWhere + " also this: " + partnerSpeech.noFishHere.Length);
            partnerSpeech.PartnerSaysSomething(partnerSpeech.noFishHere[timesFishedNoWhere]);
            timesFishedNoWhere++;
        }
    }

    #endregion
    void HideTool()
	{
		//disable collider to not select it again and disable mesh renderer to make it invisible
		GetComponent<Collider>().enabled = false;
		GetComponentInChildren<MeshRenderer>().enabled = false;
	}

	public void ShowTool()
	{
		//enable collider to make tool selectable and enable mesh renderer to make it visible
		GetComponent<Collider>().enabled = true;
		GetComponentInChildren<MeshRenderer>().enabled = true;
	}

    IEnumerator waitforAudioToLoadEndScene(AudioClip clip)
    {
        yield return new WaitForSeconds(clip.length);
        EC.CheckForEnding();
    }

    void setWrongToolVoicelineInt(WrongTool WrongToolVoice)
    {
        wrongTool = WrongToolVoice;
        //Debug.Log("I set the enum to: " + WrongToolVoice);
        LogMaster.WrongToolVoiceline = (int)wrongTool;
        //call the log entry function here inorder not to loose the wrongtool data
        if(LogMaster.filePath != null)
        {
            logMaster.logEntry(
                logMaster.player.position,
                logMaster.player.rotation.eulerAngles,
                PartnerSpeech.lastVoiceline,
                LogMaster.TypeOfFishCaught,
                GuidanceSounds.lastGuidanceSound,
                timesFishedNowhereTotal,
                LogMaster.WrongToolVoiceline);
            Debug.Log("An entry is made in the log file...");
        }
    }

    void setFishCaughtInt(FishCaught _fishcaught)
    {
        fishCaught = _fishcaught;
        LogMaster.TypeOfFishCaught = (int)fishCaught;
        //call logEntry function here inorder not to loose data
        if (LogMaster.filePath != null)
        {
            logMaster.logEntry(
                logMaster.player.position,
                logMaster.player.rotation.eulerAngles,
                PartnerSpeech.lastVoiceline,
                LogMaster.TypeOfFishCaught,
                GuidanceSounds.lastGuidanceSound,
                timesFishedNowhereTotal,
                LogMaster.WrongToolVoiceline);
            Debug.Log("An entry is made in the log file...");
        }
    }
}
