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

    private Transform mapCameraPos;
    private Transform mapOriginalPos;

    EventCatcher EC;
    PartnerAnimator PA;
    PartnerSpeech partnerSpeech;
    GuidanceSounds guidance;

	string tool ="";

    static bool wrongToolOneHook = true;
    static bool wrongToolOneSpear = true;

    private static bool endGame = false;
    public static bool firstFishingTorsk = true;
    private static bool secondFishingTorsk = false;
    private static bool thirdFishingTorsk = false;
    public static bool firstFishingEel = true;
    private static bool secondFishingEel = false;
    private static bool thirdFishingEel = false;
    public static bool firstFishingFlatfish = true;
    private static bool secondFishingFlatfish = false;
    private static bool thirdFishingFlatfish = false;

    void Start () {
        eelTrapEmptied = false;
        timesFishedNowhereTotal = 0;

        wrongToolOneHook = true;
        wrongToolOneSpear = true;

        endGame = false;
        firstFishingTorsk = true;
        secondFishingTorsk = false;
        thirdFishingTorsk = false;
        firstFishingEel = true;
        secondFishingEel = false;
        thirdFishingEel = false;
        firstFishingFlatfish = true;
        secondFishingFlatfish = false;
        thirdFishingFlatfish = false;

        if (GameManager.singleton != null)
        {
            
        }
		try
		{
            EC = GameManager.singleton.boat.GetComponent<EventCatcher>();
			PA = GameManager.singleton.partner.GetComponent<PartnerAnimator>();
            partnerSpeech = GameManager.singleton.partner.GetComponent<PartnerSpeech>();
            guidance = GameManager.singleton.CameraContainer.GetComponent<GuidanceSounds>();
        }
		catch{}
        timesFishedNoWhere = 0;
    }
	
	public void Select(){
		//selecting which tool to use 
		if(tag == "hook")
		{
			tool = tag;
			//play animation

			if (EventCatcher.fishingArea == "TorskArea")
			{
                EC.startFishing(tool);
                if (GameManager.singleton.boat.GetComponent<EventCatcher>()
                    .fishingAreaObject.GetComponent<FishContent>().getAreaOutOfFish())
                {
                    return;                  
                }
				HideTool();
				PA.HookAnimation();

                #region partner say something depending on which number of torsk is caught
                if (firstFishingTorsk)
                {
                    partnerSpeech.PartnerSaysSomething(partnerSpeech.whileTorskFishingFirst);
                    firstFishingTorsk = false;
                    secondFishingTorsk = true;
                    return;
                }
                if (secondFishingTorsk)
                {
                    partnerSpeech.PartnerSaysSomething(partnerSpeech.whileTorskFishingSecond);
                    secondFishingTorsk = false;
                    thirdFishingTorsk = true;
                    return;
                }
                if (thirdFishingTorsk)
                {
                    partnerSpeech.PartnerSaysSomething(partnerSpeech.whileTorskFishingThird);
                    thirdFishingTorsk = false;
                    return;
                }
                #endregion
            }
            else if(EventCatcher.fishingArea == "")
			{
                //no fish here
                notEnoughFishHere();
			}
			else
			{
                //wrong tool
                if (wrongToolOneHook)
                {
                    partnerSpeech.PartnerSaysSomething(partnerSpeech.NoHook4EelTryIron);
                    wrongToolOneHook = !wrongToolOneHook;
                }
                else
                {
                    partnerSpeech.PartnerSaysSomething(partnerSpeech.NoHook4EelTryIron2);
                    wrongToolOneHook = !wrongToolOneHook;
                }
			}
		}
		else if (tag == "eeliron")
		{
			tool = tag;
			//play animation

			if (EventCatcher.fishingArea == "EelArea")
			{
                EC.startFishing(tool);
                if (GameManager.singleton.boat.GetComponent<EventCatcher>().fishingAreaObject.GetComponent<FishContent>().getAreaOutOfFish())
                {
                    return;
                }
                HideTool();
				PA.EelironAnimation();

                #region partner says something depending on which number of eel is caught
                if (firstFishingEel)
                {
                    partnerSpeech.PartnerSaysSomething(partnerSpeech.whileEelFishingFirst);
                    firstFishingEel = false;
                    secondFishingEel = true;
                    return;
                }
                if (secondFishingEel)
                {
                    partnerSpeech.PartnerSaysSomething(partnerSpeech.whileEelFishingSecond);
                    secondFishingEel = false;
                    thirdFishingEel = true;
                    return;
                }
                if (thirdFishingEel)
                {
                    partnerSpeech.PartnerSaysSomething(partnerSpeech.whileEelFishingThird);
                    thirdFishingEel = false;
                    return;
                }
                #endregion
            }
            else if(EventCatcher.fishingArea == "FlatfishArea")
            {
                EC.startFishing(tool);
                if (GameManager.singleton.boat.GetComponent<EventCatcher>().fishingAreaObject.GetComponent<FishContent>().getAreaOutOfFish())
                {
                    return;
                }
                HideTool();
                PA.EelironAnimation();

                #region partner says something depending on which number of flatfish is caught
                if (firstFishingFlatfish)
                {
                    partnerSpeech.PartnerSaysSomething(partnerSpeech.whileFlatfishFihingFirst);
                    firstFishingFlatfish = false;
                    secondFishingFlatfish = true;
                    return;
                }
                if (secondFishingFlatfish)
                {
                    partnerSpeech.PartnerSaysSomething(partnerSpeech.whileFlatfishFihingSecond);
                    secondFishingFlatfish = false;
                    thirdFishingFlatfish = true;
                    return;
                }
                if (thirdFishingFlatfish)
                {
                    partnerSpeech.PartnerSaysSomething(partnerSpeech.whileFlatfishFihingThird);
                    thirdFishingFlatfish = false;
                    return;
                }
                #endregion
            }
            else if(EventCatcher.fishingArea == "")
			{
                //no fish here
                notEnoughFishHere();
			}
			else if(EventCatcher.fishingArea == "TorskArea")
			{
                //wrong tool
                if (wrongToolOneSpear == true)
                {
                    partnerSpeech.PartnerSaysSomething(partnerSpeech.NoIron4CodTryHook);
                    wrongToolOneSpear = !wrongToolOneSpear;
                }
                else
                {
                    partnerSpeech.PartnerSaysSomething(partnerSpeech.NoIron4CodTryHook2);
                    wrongToolOneSpear = !wrongToolOneSpear;
                }
			}			
		}
    }

    #region empty eeltrap 
    //PA.BasketAnimation is apparently what empties the basket and adds fish to the basket, or well it makes it 
    //somehow transition to a function in PartnerAnimator called BasketAniDone() which takes care of 
    //adding and instanciating the fish.
    public void EmptyBasket()
	{
		if(tag =="emptyBasket")
		{
            GameManager.singleton.canMove = false;

			PA.BasketAnimation();
            partnerSpeech.PartnerSaysSomething(partnerSpeech.whileEmptyingEeltrap);

            eelTrapEmptied = true;
			GetComponent<Collider>().enabled=false;
        }
	}
    #endregion

    public void EndGame()
    {
        if (tag == "EndGame")
        {
            if (endGame)
            {
                partnerSpeech.PartnerSaysSomething(partnerSpeech.endIsConfirmed);
                //waiting to load end scene till the audioclip is done playing
                waitforAudioToLoadEndScene(partnerSpeech.endIsConfirmed);
                return;
            }
            //play voiceline "are you sure you want to end"
            partnerSpeech.PartnerSaysSomething(partnerSpeech.confirmEndPlz);
            endGame = true;
            StartCoroutine(resetEnding());
        }
    }

    public void notEnoughFishHere()
    {
        //for log
        timesFishedNowhereTotal++;

        //functionallity
        if (timesFishedNoWhere < partnerSpeech.noFishHere.Length)
        {
            partnerSpeech.PartnerSaysSomething(partnerSpeech.noFishHere[timesFishedNoWhere]);
            timesFishedNoWhere++;
        }
        if (timesFishedNoWhere >= partnerSpeech.noFishHere.Length)
        {
            if(GameManager.singleton.useGuidanceSounds)
                guidance.PlayGuidanceSound();
            timesFishedNoWhere = 0;
        }
        
    }

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

    private void waitforAudioToLoadEndScene(AudioClip clip)
    {
        Object.FindObjectOfType<SceneLoadManager>().ChangeScene(clip.length);
        EC.CheckForEnding();
    }

    IEnumerator resetEnding()
    {
        yield return new WaitForSeconds(15);
        endGame = false;
    }
}
