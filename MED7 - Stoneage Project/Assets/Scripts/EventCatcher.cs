using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventCatcher : MonoBehaviour {
    public PartnerSpeech partnerSpeech;

	public static string fishingArea = "none";

    [Range(0,10)]
    public float OutOfBoundsSoundTimer = 7f;

    public GameObject fishingAreaObject;

    private int _fishToSucceed;

	private bool canFish;
    private bool hasFlint =false;
    private bool canPlayTurnAroundSound = true;
    private GuidanceSounds guidanceSounds;

    private void Start()
    {
        _fishToSucceed = GameManager.singleton.FishWinAmount;
        canFish = false;

        hasFlint = false;

        canPlayTurnAroundSound = true;
        guidanceSounds = FindObjectOfType<GuidanceSounds>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Starting area")
        {
        }
        if (other.tag == "turnAround" && canPlayTurnAroundSound)
		{
			GameManager.singleton.partner.GetComponent<PartnerSpeech>().PartnerSaysSomething(
				GameManager.singleton.partner.GetComponent<PartnerSpeech>().NoFurther);
            canPlayTurnAroundSound = false;
            StartCoroutine(outOfBoundsSoundDelay(OutOfBoundsSoundTimer));
		}
		if(other.tag == "TorskArea" || other.tag == "EelArea" || other.tag == "FlatfishArea")
		{
            Debug.Log(name +" entered with area called " + other.tag +" on object called " + other.name);
            canFish = true;
            //play can fish sound
            if(other.tag == "TorskArea" && SelectTool.firstFishingTorsk)
                partnerSpeech.PartnerSaysSomething(partnerSpeech.canFishHere, false);
            if(other.tag == "EelArea" && SelectTool.firstFishingEel)
                partnerSpeech.PartnerSaysSomething(partnerSpeech.canFishHere, false);
            if(other.tag == "FlatfishArea" && SelectTool.firstFishingFlatfish)
                partnerSpeech.PartnerSaysSomething(partnerSpeech.canFishHere, false);

            guidanceSounds.AddTimeToGuidanceActiveTimer(5f);
			fishingArea = other.tag;
			fishingAreaObject = other.gameObject;		
		}
        //when you go back to ertebølle midden to retrieve tool
        if (other.tag == "pelicanTrigger")
        {
            GameManager.singleton.PelicanEvent.SetActive(true);
            GameManager.singleton.PelicanEvent.transform.SetParent(null);
            //remains from old project... don't fix if not broken....
            GameManager.singleton.PelicanEvent.GetComponentInChildren<PelicanEvent>().startOrcaEvent();
            //partner says pelican thing
        }
    }

	void OnTriggerExit(Collider other)
    {
        //when you exit a fishing area, you are set to not be able to fish anymore
        if(other.tag == "TorskArea" || other.tag == "EelArea" || other.tag == "FlatfishArea")
        {
            ExitArea();
        }
        //when exit torsk territory pelican event happens      
    }

	public void CheckForEnding()
	{
		if(GameManager.singleton.GetFishCount() >= _fishToSucceed)
			{
				GameManager.singleton.PrepareForEndScene(
                    GameManager.singleton.partner.GetComponent<PartnerSpeech>().GoodEnding, hasFlint);
			}
			else 
			{
            GameManager.singleton.PrepareForEndScene(
                GameManager.singleton.partner.GetComponent<PartnerSpeech>().BadEnding, hasFlint);				
			}
	}

	public void ExitArea()
	{
		canFish = false;
			fishingArea = "none";
	}

	public void startFishing(string tool)
	{
		if(canFish)
		{
            fishingAreaObject.GetComponent<FishContent>().RemoveFish();            
        }
	}

	public void DisableTrading()
	{
		GameManager.singleton.tradingObject.GetComponent<Collider>().enabled = false;
	}

	public bool GetHasFlint()
	{
		return hasFlint;
	}

	public bool GetCanFish()
    {
        return canFish;
    }

    public GameObject GetCurrentFishingArea()
    {
        return fishingAreaObject;
    }

    IEnumerator outOfBoundsSoundDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canPlayTurnAroundSound = true;
    }
}
