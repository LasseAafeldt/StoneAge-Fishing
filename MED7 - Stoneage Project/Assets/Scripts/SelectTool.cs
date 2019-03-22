using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTool : MonoBehaviour {

public bool IsTribeBasket;

    private static int timesFishedNoWhere;

EventCatcher EC;
PartnerAnimator PA;
    PartnerSpeech PS;
    GuidanceSounds guidance;

	string tool ="";

    private bool endGame = false;
	// Use this for initialization
	void Start () {
		EC = GameManager.singleton.boat.GetComponent<EventCatcher>();
		try
		{
			PA = GameManager.singleton.partner.GetComponent<PartnerAnimator>();
            PS = GameManager.singleton.partner.GetComponent<PartnerSpeech>();
            guidance = GameManager.singleton.CameraContainer.GetComponent<GuidanceSounds>();

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
			}
			else if(GameManager.singleton.boat.GetComponent<EventCatcher>().fishingArea == "")
			{
                //no fish here
                notEnoughFishHere();

				/*GameManager.singleton.partner.GetComponent<PartnerSpeech>().PartnerSaysSomething(
					GameManager.singleton.partner.GetComponent<PartnerSpeech>().NotEnoughFishHere);*/
			}
			else
			{
				//wrong tool
				GameManager.singleton.partner.GetComponent<PartnerSpeech>().PartnerSaysSomething(
					GameManager.singleton.partner.GetComponent<PartnerSpeech>().NoHook4CodTryIron);
			}

			//wait for animaion to end to show tool again
			//ShowTool();

			//StartCoroutine("OnAnimationComplete");

		}
		else if (tag == "eeliron")
		{
			Debug.Log("selected eeliron");
			tool = tag;
			//play animation

			if (GameManager.singleton.boat.GetComponent<EventCatcher>().fishingArea == "EelArea" || GameManager.singleton.boat.GetComponent<EventCatcher>().fishingArea == "FlatfishArea"	)
			{
				HideTool();
				PA.EelironAnimation();
			}
			else if(GameManager.singleton.boat.GetComponent<EventCatcher>().fishingArea == "")
			{
                //no fish here
                notEnoughFishHere();

				/*GameManager.singleton.partner.GetComponent<PartnerSpeech>().PartnerSaysSomething(
					GameManager.singleton.partner.GetComponent<PartnerSpeech>().NotEnoughFishHere);*/
			}
			else
			{
				//wrong tool
				GameManager.singleton.partner.GetComponent<PartnerSpeech>().PartnerSaysSomething(
					GameManager.singleton.partner.GetComponent<PartnerSpeech>().NoIron4CodTryHook);
			}
			//wait for animaion to end to show tool again
			//ShowTool();
			
		}
		//admit to fish with the selected tool
		EC.startFishing(tool);
	}

    #region empty basket 
    //PA.BasketAnimation is apparently what empties the basket and adds fish to the basket, or well it makes it somehow
        // transition to a function in PartnerAnimator called BasketAniDone() which takes care of adding and instanciating the fish.
    public void EmptyBasket()
	{
		if(tag =="emptyBasket")
		{
            GameManager.singleton.canMove = false;
            Debug.Log("I can't move now");
			PA.BasketAnimation();
			//Debug.Log("emptying basket for test");
			GetComponent<Collider>().enabled=false;
			/*if(GameManager.singleton.Islinear)
			{
				Debug.Log("Linear empty basket");
				GameManager.singleton.
					partner.GetComponent<PartnerSpeech>().PartnerSaysSomething(
					GameManager.singleton.partner.GetComponent<PartnerSpeech>().EmptyBasket);
			}
			if(IsTribeBasket)
			{
				GameManager.singleton.tribeBoat.GetComponent<TribeController>().SetFollowPlayer(true);
				if(!GameManager.singleton.Islinear)
				{
					GameManager.singleton.
						partner.GetComponent<PartnerSpeech>().PartnerSaysSomething(
						GameManager.singleton.partner.GetComponent<PartnerSpeech>().FishingTribe);
				}				
			}*/
		}
	}

    public void EndGame()
    {
        if (tag == "EndGame")
        {
            if (endGame)
            {
                Debug.Log("Play is moving on to end scene");
                EC.CheckForEnding();
                return;
            }
            Debug.Log("player is attempting to end game");
            //play voiceline "are you sure you want to end"
            endGame = true;

        }
    }

    public void notEnoughFishHere()
    {
        if(timesFishedNoWhere >= PS.noFishHere.Length)
        {
            guidance.playGuidanceSound();
            timesFishedNoWhere = 0;
            return;
        }
        if(timesFishedNoWhere < PS.noFishHere.Length)
        {
            Debug.Log("no fish here array: " + timesFishedNoWhere + " also this: " + PS.noFishHere.Length);
            PS.PartnerSaysSomething(PS.noFishHere[timesFishedNoWhere]);
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

}
