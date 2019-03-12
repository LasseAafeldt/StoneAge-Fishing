using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartnerSpeech : MonoBehaviour {

	AudioSource audio;

	//for linear condition
	[Header("linear sounds")]
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
	public AudioClip Outcome2Linear;

	//for emergent condition
	[Header("Emergent sounds")]
	public AudioClip StartofGameEmergent;
	public AudioClip CheckBasketFish;
	public AudioClip CheckBasketNoFish;
	public AudioClip EnterCodAreaEmergent;
	public AudioClip EnterCoastAreaDay;
	public AudioClip EnterCoastAreaNight;
	public AudioClip EnterTribe;
	public AudioClip ExitTribe;
	public AudioClip FishingTribe;
	public AudioClip MeetingTribeCaught;
	public AudioClip MeetingTribeEscaped;
	public AudioClip MeetingTribeSpotTrade;
	public AudioClip MeetingTriibeStoleFish;
	public AudioClip MeeetingTribeTrade;
	public AudioClip FirstTimeEel;
	public AudioClip FirstTimeCod;
	public AudioClip FirstTimeFlatFish;
	public AudioClip SealAppearsEmergent;
	public AudioClip Time1MinLeft;
	public AudioClip Time2MinLeft;
	public AudioClip Outcome1Emergent;
	public AudioClip Outcome2Emergent;
	public AudioClip Outcome3Emergent;
	public AudioClip Outcome4Emergent;
	public AudioClip Outcome5Emergent;

	//for both condition
	[Header("neutral sounds")]
	public AudioClip OrcaAppears;
	public AudioClip PelicanAppears;
	public AudioClip SealAppears;
	public AudioClip StartofGame;
	public AudioClip ThisWay1;
	public AudioClip ThisWay2;

	//new sounds
	[Header("new sounds")]
	public AudioClip AnotherCod;
	public AudioClip AnotherEel;
	public AudioClip AnotherFlatfish;
	public AudioClip Catch1Eel;
	public AudioClip Catch2Eel;
	public AudioClip Catch3Eel;
	public AudioClip DarkSoon;
	public AudioClip GoSomewhereElse;
	public AudioClip NoFurther;
	public AudioClip NoIron4CodTryHook;
	public AudioClip NoHook4CodTryIron;
	public AudioClip NotEnoughFishHere;
	public AudioClip WeNeedFish;



	[Header(" sounds")]

	public Text speech;
	List<AudioClip> queuedAudio = new List<AudioClip>();
	List<string> queuedText = new List<string>();



	bool donePlaying =true;
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();

		PartnerSaysSomething(StartofGame);
		speech.text = "";
		if(GameManager.singleton.Islinear)
		{
			PartnerSaysSomething(StartofGameLinear, "TØM ÅLEGÅRDEN");
		}
		else
		{
			PartnerSaysSomething(StartofGameEmergent, "");
		}

	}
	
	// Update is called once per frame
	void Update () {
		

		if (!audio.isPlaying && !donePlaying)
        {
            //Debug.Log(audio.clip.name);
            //audioSource.Play();
			donePlaying = true;
			GetComponent<PartnerAnimator>().StopTalking();
			if(audio.clip.name == "StartofGame_linear")
			{
				GameManager.singleton.pillar1.SetActive(true);
				GameManager.singleton.currentPillar =GameManager.singleton.pillar1;
				GameManager.singleton.basket.GetComponent<Collider>().enabled = true;
			}
			else if(audio.clip.name == "AfterEmptyBasket_linear")
			{
				GameManager.singleton.pillar2.SetActive(true);
				GameManager.singleton.currentPillar =GameManager.singleton.pillar2;
				GameManager.singleton.pillar1.SetActive(false);
				//Destroy(GameManager.singleton.pillar1);
				Debug.Log("pillar1 is now "+GameManager.singleton.pillar1.activeSelf);
				GameManager.singleton.torskTerritory.GetComponent<Collider>().enabled = true;
				GameManager.singleton.StartCountingTorsk();
				foreach(GameObject area in GameManager.singleton.torskArea)
				{
					Debug.Log("activating area");
					area.SetActive(true);
				}
				//start counting Cod
			}
			else if(audio.clip.name == "AfterCodCatch_linear")
			{
				GameManager.singleton.pillar3.SetActive(true);
				GameManager.singleton.currentPillar =GameManager.singleton.pillar3;
				GameManager.singleton.pillar2.SetActive(false);
				Debug.Log("pillar2 is now "+GameManager.singleton.pillar2.activeSelf);
				GameManager.singleton.trading.GetComponent<Collider>().enabled = true;
				GameManager.singleton.torskTerritory.GetComponent<Collider>().enabled = false;

			}
			else if(audio.clip.name == "AfterTradingFlint_linear2")
			{
				Debug.Log("eel event happening");
				GameManager.singleton.pillar4.SetActive(true);
				GameManager.singleton.currentPillar =GameManager.singleton.pillar4;
				GameManager.singleton.pillar3.SetActive(false);
				Debug.Log("pillar3 is now "+GameManager.singleton.pillar3.activeSelf);
				GameManager.singleton.eelTerritory.GetComponent<Collider>().enabled = true;
				GameManager.singleton.torskTerritory2.GetComponent<Collider>().enabled = true;
				GameManager.singleton.StartCountingEel();
				GameManager.singleton.tribeBoat.GetComponent<TribeController>().GetInPosition(GameManager.singleton.bjørnsholm.transform.position);
				foreach(GameObject area in GameManager.singleton.eelArea)
				{
					area.SetActive(true);
				}
				//start counting eel
			}
			else if(audio.clip.name == "AfterFlaringEel_linear")
			{
				GameManager.singleton.pillar5.SetActive(true);
				GameManager.singleton.currentPillar =GameManager.singleton.pillar5;
				GameManager.singleton.pillar4.SetActive(false);
				Debug.Log("pillar4 is now "+GameManager.singleton.pillar4.activeSelf);
				GameManager.singleton.midden.GetComponent<Collider>().enabled = true;
			}
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
			}
		}
		else
		{
			GetComponent<PartnerAnimator>().StartTalking();
			audio.clip = clip;
			speech.text = writtenLine;
			audio.Play();
			
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
			}

		}
		else
		{
			GetComponent<PartnerAnimator>().StartTalking();
			audio.clip = clip;
			audio.Play();
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
		}

	}

}
