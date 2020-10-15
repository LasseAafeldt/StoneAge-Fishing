using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventCatcher : MonoBehaviour {
    public PartnerSpeech partnerSpeech;

	public string fishingArea;

    [Range(0,10)]
    public float OutOfBoundsSoundTimer = 7f;

    public GameObject fishingAreaObject;

    private int _fishToSucceed;

	private bool canFish;
    private bool hasFlint =false;
    private bool canPlayTurnAroundSound = true;
    private GuidanceSounds guidanceSounds = new GuidanceSounds();//if used for anything else than adding time
    //to timer then we should get the specific one in the scene instead.

    private void Start()
    {
        _fishToSucceed = GameManager.singleton.FishWinAmount;
        canFish = false;

        hasFlint = false;

        canPlayTurnAroundSound = true;
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
            //Debug.Log("player is hitting the boundry of the map and No Further sound is played");
            canPlayTurnAroundSound = false;
            StartCoroutine(outOfBoundsSoundDelay(OutOfBoundsSoundTimer));
		}
		if(other.tag == "TorskArea" || other.tag == "EelArea" || other.tag == "FlatfishArea")
		{
            canFish = true;
            //play can fish sound
            if(other.tag == "TorskArea" && SelectTool.firstFishingTorsk)
                partnerSpeech.PartnerSaysSomething(partnerSpeech.canFishHere);
            if(other.tag == "EelArea" && SelectTool.firstFishingEel)
                partnerSpeech.PartnerSaysSomething(partnerSpeech.canFishHere);
            if(other.tag == "FlatfishArea" && SelectTool.firstFishingFlatfish)
                partnerSpeech.PartnerSaysSomething(partnerSpeech.canFishHere);

            //Debug.Log("<color=orange>"+ other.gameObject.name + " Collides with " + gameObject.name + "</Color>");
            guidanceSounds.AddTimeToGuidanceActiveTimer(5f);
			fishingArea = other.tag;
			fishingAreaObject = other.gameObject;
			//Debug.Log("you are now in the "+fishingArea);			
		}
        //when you go back to ertebølle midden to retrieve tool
		if(other.tag == "ertebølle")
		{
            //Debug.Log("player sailed into the Ertebølle trigger: if you want to end then hand the fish over to the lady");
		}
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
            //enough fish 7
            //Debug.Log("Good ending");
				GameManager.singleton.PrepareForEndScene(GameManager.singleton.partner.GetComponent<PartnerSpeech>().GoodEnding, hasFlint);
				SceneManager.LoadScene("End Scene", LoadSceneMode.Single);
				
			}
			else 
			{
            //not enough fish 7
            //Debug.Log("Bad ending");
            GameManager.singleton.PrepareForEndScene(GameManager.singleton.partner.GetComponent<PartnerSpeech>().BadEnding, hasFlint);
				SceneManager.LoadScene("End Scene", LoadSceneMode.Single);				
			}
        LogMaster.shouldBeLogging = false;
        //GameManager.singleton.logMaster.enabled = false;
        Debug.Log("Logging is disabled...");
	}

	public void ExitArea()
	{
		canFish = false;
			fishingArea = "";
	}

	public void startFishing(string tool)
	{
		//Debug.Log("trying to fish");
		if(canFish)
		{
            fishingAreaObject.GetComponent<FishContent>().RemoveFish();            
        }

	}


	public void DisableTrading()
	{
		GameManager.singleton.tradingObject.GetComponent<Collider>().enabled = false;
	}

	/*public void TradeFishForFlint()
	{
		Debug.Log("you want to trade" + GameManager.singleton.currentEelAmount);
		if(GameManager.singleton.currentEelAmount >= 4)
		{
			Debug.Log("You have enough fish");
			GameObject currentFish;

			for (int i = 1; i < 5; i++)
			{

				Transform[] trans = GameObject.FindGameObjectWithTag("basket").GetComponentsInChildren<Transform>(true);
				foreach (Transform t in trans) {
					if (t.gameObject.name == "eel_Caught_0"+i) 
					{
						currentFish = t.gameObject;
						currentFish.SetActive(false);
						GameManager.singleton.currentEelAmount -= 1;
						Debug.Log("ÅL: " + GameManager.singleton.currentEelAmount);
					}
				}
				
			}


			//GameManager.singleton.RemoveAnyFish(5);
			
			Instantiate(GameManager.singleton.flint,GameObject.FindGameObjectWithTag("basket").transform.position-1.5f*transform.forward, transform.rotation,GameObject.FindGameObjectWithTag("basket").transform);
			hasFlint = true;
			DisableTrading();
		}

	}*/

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
