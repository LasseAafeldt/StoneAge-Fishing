using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventCatcher : MonoBehaviour {
    public PartnerSpeech partnerSpeech;

	bool canFish;
	public string fishingArea;

    [Range(0,10)]
    public int FishToSucceed = 7;


    bool firstTimeInTorskArea = true;
	bool firstTimeInEelArea = true;

    private bool tribeTerritory = false;

    GameObject fishingAreaObject;

	bool hasFlint=false;

	void OnTriggerEnter(Collider other)
    {
		if(other.tag == "turnAround")
		{
			GameManager.singleton.partner.GetComponent<PartnerSpeech>().PartnerSaysSomething(
				GameManager.singleton.partner.GetComponent<PartnerSpeech>().NoFurther);
            Debug.Log("player is hitting the boundry of the map and No Further sound is played");
		}
		if(other.tag == "TorskArea" || other.tag == "EelArea" || other.tag == "FlatfishArea")
		{
			canFish = true;
			fishingArea = other.tag;
			fishingAreaObject = other.gameObject;
			Debug.Log("you are now in the "+fishingArea);
			
			if(other.tag == "TorskArea" && firstTimeInTorskArea)
			{
                //Debug.Log("I have entered the torsk area first time");
				firstTimeInTorskArea = false;
				/*GameManager.singleton.
						partner.GetComponent<PartnerSpeech>().PartnerSaysSomething(
						GameManager.singleton.partner.GetComponent<PartnerSpeech>().EnterCodAreaEmergent);*/					
			}
			if( other.tag == "EelArea" && firstTimeInEelArea)
			{
				firstTimeInEelArea = false;
				/*float time =GameManager.singleton.timer.GetComponent<playTimer>().GetTimeSpent();
					if(time > 0.4)
					{
						GameManager.singleton.
							partner.GetComponent<PartnerSpeech>().PartnerSaysSomething(
							GameManager.singleton.partner.GetComponent<PartnerSpeech>().EnterCoastAreaDay);
					}

					else
					{
						GameManager.singleton.
							partner.GetComponent<PartnerSpeech>().PartnerSaysSomething(
							GameManager.singleton.partner.GetComponent<PartnerSpeech>().EnterCoastAreaNight);
					}
				}*/
			}
		}
        //when you go back to ertebølle midden to retrieve tool
		if(other.tag == "ertebølle")
		{
            //change scene
            //CheckForEnding();
            //Debug.Log("I have sailed into the collider which previously caused the game to load the endscene");
            partnerSpeech.PartnerSaysSomething(partnerSpeech.HomeAgain);
            Debug.Log("player sailed intot the Ertebølle trigger: if you want to end then hand the fish over to the lady");
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
        /*if(other.tag == "pelicanTrigger")
        {
			GameManager.singleton.PelicanEvent.SetActive(true);
			GameManager.singleton.PelicanEvent.transform.SetParent(null);
			GameManager.singleton.PelicanEvent.GetComponentInChildren<orcaEvent>().startOrcaEvent();
            //partner says pelican thing
        }*/       
    }

	public void CheckForEnding()
	{
		if(GameManager.singleton.GetFishCount() >=FishToSucceed)
			{				
				//enough fish 7
				GameManager.singleton.PrepareForEndScene(GameManager.singleton.partner.GetComponent<PartnerSpeech>().GoodEnding, hasFlint);
				SceneManager.LoadScene("End Scene", LoadSceneMode.Single);
				
			}
			else 
			{				
				//not enough fish 7
				GameManager.singleton.PrepareForEndScene(GameManager.singleton.partner.GetComponent<PartnerSpeech>().BadEnding, hasFlint);
				SceneManager.LoadScene("End Scene", LoadSceneMode.Single);				
			}
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
			if(fishingArea == "TorskArea")
			{
				if (tool == "hook")
				{
					Debug.Log("caugth a torsk");
					//instatiate a fish in the boay
					//Instantiate(torsk,transform.position, transform.rotation);
					//remove a fish from the ocean
					fishingAreaObject.GetComponent<FishContent>().RemoveFish();
				}
				else
				{

				}

			}
			if(tool == "eeliron" && fishingArea == "EelArea")
			{
				Debug.Log("caugth a eel");
				//instatiate a fish in the boay
				//Instantiate(eel,transform.position, transform.rotation);
				//remove a fish from the ocean
				fishingAreaObject.GetComponent<FishContent>().RemoveFish();
			}
			else
			{
				//partner should make a comment
			}
			
		}

	}


	public void DisableTrading()
	{
		GameManager.singleton.tradingObject.GetComponent<Collider>().enabled = false;
	}

	public void TradeFishForFlint()
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


}
