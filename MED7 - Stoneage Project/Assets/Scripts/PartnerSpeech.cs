using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartnerSpeech : MonoBehaviour {

	AudioSource audio;
    public static int amountOfVoiceLinesPlayed = 0;
    public LogMaster logMaster;

	//for linear condition
	/*[Header("linear sounds")]
	public AudioClip StartofGameLinear;
	public AudioClip AfterEmptyBasket;
	public AudioClip AfterCodCatch;
	public AudioClip AfterTradingFlint;
	public AudioClip AfterFlaringEel;
	public AudioClip CodEnterArea;
	public AudioClip CodOneMore;
	public AudioClip CodTwoMore;
	public AudioClip EmptyBasket;
	public AudioClip FlaringEel;
	public AudioClip NotThatWay;
	public AudioClip OtherTribeSpotTrade;
	public AudioClip Outcome1Linear;
	public AudioClip Outcome2Linear;*/

	//for emergent condition
	[Header("Emergent sounds")]
	//public AudioClip StartofGameEmergent;
	//public AudioClip CheckBasketFish;
	//public AudioClip CheckBasketNoFish;
	//public AudioClip EnterCodAreaEmergent;
	//public AudioClip EnterCoastAreaDay;
	//public AudioClip EnterCoastAreaNight;
	//public AudioClip EnterTribe;
	//public AudioClip ExitTribe;
	//public AudioClip FishingTribe;
	//public AudioClip MeetingTribeCaught;
	//public AudioClip MeetingTribeEscaped;
	//public AudioClip MeetingTribeSpotTrade;
	//public AudioClip MeetingTriibeStoleFish;
	//public AudioClip MeeetingTribeTrade;
	//public AudioClip FirstTimeEel;
	//public AudioClip FirstTimeCod;
	//public AudioClip FirstTimeFlatFish;
	public AudioClip SealAppearsEmergent;
	//public AudioClip Time1MinLeft;
	//public AudioClip Time2MinLeft;
	public AudioClip BadEnding;
	//public AudioClip Outcome2Emergent;
	public AudioClip GoodEnding;
    //public AudioClip Outcome4Emergent;
    //public AudioClip Outcome5Emergent;
    public AudioClip GameTimerLow;
    public AudioClip GameTimerEnd;
    public AudioClip HomeAgain;
    public AudioClip confirmEndPlz;
    public AudioClip endIsConfirmed;
    public AudioClip whileEelFishingFirst;
    public AudioClip whileEelFishingSecond;
    public AudioClip whileEelFishingThird;
    public AudioClip whileTorskFishingFirst;
    public AudioClip whileTorskFishingSecond;
    public AudioClip whileTorskFishingThird;
    public AudioClip whileFlatfishFihingFirst;
    public AudioClip whileFlatfishFihingSecond;
    public AudioClip whileFlatfishFihingThird;
    public AudioClip whileEmptyingEeltrap;
    public AudioClip afterEmptyingEeltrap;

    //for both condition
    [Header("neutral sounds")]
	public AudioClip OrcaAppears;
	public AudioClip PelicanAppears;
	/*public AudioClip SealAppears;
	public AudioClip StartofGame;
	public AudioClip ThisWay1;
	public AudioClip ThisWay2;*/

	//new sounds
	[Header("Warning clips")]
	/*public AudioClip AnotherCod;
	public AudioClip AnotherEel;
	public AudioClip AnotherFlatfish;
	public AudioClip Catch1Eel;
	public AudioClip Catch2Eel;
	public AudioClip Catch3Eel;
	public AudioClip DarkSoon;
	public AudioClip GoSomewhereElse;*/
	public AudioClip NoFurther;
	public AudioClip NoIron4CodTryHook;
	public AudioClip NoHook4CodTryIron;
	//public AudioClip NotEnoughFishHere;
	//public AudioClip WeNeedFish;

    [Header("Guidance sounds")]
    public AudioClip ClosestToTorsk;
    public AudioClip DetailClosestToTorsk;
    public AudioClip ClosestToEel;
    public AudioClip DetailClosestToEel;
    public AudioClip ClosestToEeltrap;
    public AudioClip DetailClosestToEeltrap;
    public AudioClip ClosestToFlatfish;
    public AudioClip DetailClosestToFlatfish;
    public AudioClip StartingSoundGoFishing;

    [Header(" sounds")]
    public AudioClip[] noFishHere;
    public Text speech;
	List<AudioClip> queuedAudio = new List<AudioClip>();
	List<string> queuedText = new List<string>();



	bool donePlaying =true;
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		

		if (!audio.isPlaying && !donePlaying)
        {
            //Debug.Log(audio.clip.name);
            //audioSource.Play();
			donePlaying = true;
			GetComponent<PartnerAnimator>().StopTalking();	
			
        }
		else if(!audio.isPlaying && queuedAudio.Count!=0)
		{
			if(queuedText.Count!=0)
				{
					PartnerSaysSomething(queuedAudio[0],queuedText[0]);
					queuedAudio.Remove(queuedAudio[0]);
					queuedText.Remove(queuedText[0]);
				}
				else 
				{
					PartnerSaysSomething(queuedAudio[0]);
					queuedAudio.Remove(queuedAudio[0]);
				}
		}
		else if (audio.isPlaying && donePlaying)
		{
			donePlaying=false;
			//Debug.Break();

		}
	}


	public void PartnerSaysSomething(AudioClip clip, string writtenLine)
	{
		if(audio.isPlaying)
		{
			if(!queuedAudio.Contains(clip) && audio.clip.name != clip.name)
			{
				queuedAudio.Add(clip);
				queuedText.Add(writtenLine);

                //amountOfVoiceLinesPlayed++;
			}
		}
		else
		{
			GetComponent<PartnerAnimator>().StartTalking();
			audio.clip = clip;
			speech.text = writtenLine;
			audio.Play();
            amountOfVoiceLinesPlayed++;
            //sound is played so do log entry
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

	}
	public void PartnerSaysSomething(AudioClip clip, string writtenLine, bool animation)
	{
		if(audio.isPlaying)
		{
			if(!queuedAudio.Contains(clip) && audio.clip.name != clip.name)
			{
				queuedAudio.Add(clip);
				queuedText.Add(writtenLine);
								Debug.Log(audio.clip.name + " audio - clip " + clip.name);
                //amountOfVoiceLinesPlayed++;
			}

		}
		else
		{
			if (animation)
			{
				GetComponent<PartnerAnimator>().StartTalking();
			}
			audio.clip = clip;
			speech.text = writtenLine;
			audio.Play();
            amountOfVoiceLinesPlayed++;
            //sound is played so do log entry
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

	}
	public void PartnerSaysSomething(AudioClip clip)
	{
		if(audio.isPlaying)
		{
			if(!queuedAudio.Contains(clip) && audio.clip.name != clip.name)
			{
				queuedAudio.Add(clip);
				Debug.Log(audio.clip.name + " audio - clip " + clip.name);
                //amountOfVoiceLinesPlayed++;
			}

		}
		else
		{
			GetComponent<PartnerAnimator>().StartTalking();
			audio.clip = clip;
			audio.Play();
            amountOfVoiceLinesPlayed++;
            //sound is played so do log entry
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

	}
	public void PartnerSaysSomething(AudioClip clip, bool animation)
	{
		if(audio.isPlaying)
		{
			if(!queuedAudio.Contains(clip) && audio.clip.name != clip.name)
			{
				queuedAudio.Add(clip);
				Debug.Log(audio.clip.name + " audio1 - clip " + clip.name);
                //amountOfVoiceLinesPlayed++;
			}
		}
		else
		{
			if (animation)
			{
				GetComponent<PartnerAnimator>().StartTalking();
			}
			audio.clip = clip;
			audio.Play();
            amountOfVoiceLinesPlayed++;
            //sound is played so do log entry
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

	}

}
