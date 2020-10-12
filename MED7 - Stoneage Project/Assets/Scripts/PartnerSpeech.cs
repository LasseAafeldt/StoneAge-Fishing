using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PartnerSpeech : MonoBehaviour {
    public GuidanceSounds guidance;
	AudioSource audio;
    public static string lastVoiceline;

    //for emergent condition
    [Header("Sounds")]
	public AudioClip SealAppearsEmergent;
	public AudioClip BadEnding;
	public AudioClip GoodEnding;
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
    public AudioClip areaIsOutOfFish;
    public AudioClip canFishHere;

    [Header("Event sounds")]
	public AudioClip OrcaAppears;
	public AudioClip PelicanAppearsNoSoundPlaying;
    public AudioClip PelicanAppearsSomeSoundIsAlreadyPlaying;

    [Header("Warning clips")]
	public AudioClip NoFurther;
	public AudioClip NoIron4CodTryHook;
    public AudioClip NoIron4CodTryHook2;
    public AudioClip NoHook4EelTryIron;
    public AudioClip NoHook4EelTryIron2;

    [Header("Guidance sounds")]
    public AudioClip ClosestToTorsk;
    public AudioClip[] DetailClosestToTorsk;
    [Space]
    public AudioClip ClosestToEel;
    public AudioClip[] DetailClosestToEel;
    [Space]
    public AudioClip ClosestToEeltrap;
    public AudioClip[] DetailClosestToEeltrap;
    [Space]
    public AudioClip ClosestToFlatfish;
    public AudioClip[] DetailClosestToFlatfish;
    [Space]
    public AudioClip StartingSoundGoFishing;

    [Header(" sounds")]
    public AudioClip[] noFishHere;
    //public Text speech;
	List<AudioClip> queuedAudio = new List<AudioClip>();
	List<string> queuedText = new List<string>();

	bool donePlaying =true;
	// Use this for initialization
	void Start ()
    {
        donePlaying = true;
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
			//speech.text = writtenLine;
			audio.Play();
            //amountOfVoiceLinesPlayed++;
            setLastPlayedVoiceline(clip);
            guidance.AddTimeToGuidanceActiveTimer(clip.length);
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
			//speech.text = writtenLine;
			audio.Play();
            //amountOfVoiceLinesPlayed++;
            setLastPlayedVoiceline(clip);
            guidance.AddTimeToGuidanceActiveTimer(clip.length);
        }

	}
	public void PartnerSaysSomething(AudioClip clip)
	{
		if(audio.isPlaying)
		{
			if(!queuedAudio.Contains(clip) && audio.clip.name != clip.name)
			{
				queuedAudio.Add(clip);
				//Debug.Log(audio.clip.name + " audio - clip " + clip.name);
                //amountOfVoiceLinesPlayed++;
			}

		}
		else
		{
			GetComponent<PartnerAnimator>().StartTalking();
			audio.clip = clip;
			audio.Play();
            //amountOfVoiceLinesPlayed++;
            setLastPlayedVoiceline(clip);
            guidance.AddTimeToGuidanceActiveTimer(clip.length);
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
            //amountOfVoiceLinesPlayed++;
            setLastPlayedVoiceline(clip);
            guidance.AddTimeToGuidanceActiveTimer(clip.length);
        }

	}

    void setLastPlayedVoiceline(AudioClip clip)
    {
        lastVoiceline = clip.name;
    }
}
