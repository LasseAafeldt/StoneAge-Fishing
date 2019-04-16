using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartnerAnimator : MonoBehaviour {


    [HideInInspector]
	public Animator anim;
    public PartnerSpeech partnerSpeech;
    public GuidanceSounds guidance;
    public int EelsInTrap = 3;
    public int FlatFishInTrap = 1;
    public AudioSource basketSound;

    GameObject boat;

	GameObject mostRecentFish;

	bool firstTimeEel =true;
	bool firstTimeCod =true;
	bool firstTimeFlatfish =true;

	// Use this for initialization
	void Start () {
        firstTimeEel = true;
        firstTimeCod = true;
        firstTimeFlatfish = true;

        anim = GetComponent<Animator>();
		boat = GameManager.singleton.spawnPoint;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void HookAnimation()
	{
		anim.SetTrigger("hookFishing");
        GameManager.singleton.canMove = false;
        Debug.Log("I can't move anymore");
		GameManager.singleton.paddle.SetActive(false);
		GameManager.singleton.partner.transform.position = GameManager.singleton.partner.transform.position - 0.75f*transform.up;
	}
	public void EelironAnimation()
	{
		anim.SetTrigger("eelIronFishing");
		GameManager.singleton.paddle.SetActive(false);
		GameManager.singleton.aniEelIron.SetActive(true);
		GameManager.singleton.aniTorch.SetActive(true);
        GameManager.singleton.canMove = false;
        Debug.Log("I can't move anymore");
    }

	public void BasketAnimation()
	{
		anim.SetTrigger("basketFishing");
	}

	public void trapEmpty()
	{
		
		anim.SetTrigger("trapEmpty");
	}

	public void trapFull()
	{
		
		anim.SetTrigger("trapFull");
        
	}

	public void StartTalking()
	{
		anim.SetBool("isTalking", true);
	}
	public void StopTalking()
	{
		anim.SetBool("isTalking",false);
	}

	public void codCaught()
	{
		anim.SetTrigger("codCaught");
        GameManager.singleton.canMove = true;
        Debug.Log("I can move again");
	}

	public void eelCaught()
	{
		anim.SetTrigger("eelCaught");
        GameManager.singleton.canMove = true;
        Debug.Log("I can move again");
    }

	public void noCatch()
	{
		anim.SetTrigger("noCatch");
        GameManager.singleton.canMove = true;
        Debug.Log("I can move again");
    }
	public void pointLeft(bool state)
	{
		anim.SetBool("pointLeft", state);
	}

	public void pointRight(bool state)
	{
		anim.SetBool("pointRight", state);
	}

	public void wrongWay(bool state)
	{
		anim.SetBool("wrongWay", state);
	}

	public void paddleAnimation(bool state){
        //Debug.Log("I am paddling...");
		anim.SetBool("isRowing", state);
	}


	public void HookAniDone(){
		GameManager.singleton.hook.GetComponent<SelectTool>().ShowTool();
		GameManager.singleton.paddle.SetActive(true);
		Debug.Log("Hook ani done");
		GameManager.singleton.partner.transform.position = GameManager.singleton.partner.transform.position + 0.75f*transform.up;
		if(GameManager.singleton.boat.GetComponent<EventCatcher>().GetCanFish())
		{
			codCaught();
			PutTorskInBasket(1);
            //tosk area has now been used once
            GameManager.singleton.TorskCaught = true;
            //maybe do reset of guidance timer here????
            guidance.resetGuidanceTimers();
        } else {
			noCatch();
		}

		//check if there is more fish in the area
		try
		{
			GameManager.singleton.boat.GetComponent<EventCatcher>().GetCurrentFishingArea().GetComponent<FishContent>().DestroyEmptyArea();
		}
		catch{}

	}
	public void PaddleAniDone(){
		ResetToolOnGuide();

	}
	
	public void EelIronAniDone(){
		GameManager.singleton.eeliron.GetComponent<SelectTool>().ShowTool();
		GameManager.singleton.paddle.SetActive(true);
		GameManager.singleton.aniEelIron.SetActive(false);
		GameManager.singleton.aniTorch.SetActive(false);
		Debug.Log("iron ani done");
		if(GameManager.singleton.boat.GetComponent<EventCatcher>().GetCanFish())
		{
			eelCaught();
			if(GameManager.singleton.boat.GetComponent<EventCatcher>().GetCurrentFishingArea().tag == "EelArea")
			{
				
				PutEelInBasket(1,true);
                GameManager.singleton.eelCaught = true;
                guidance.resetGuidanceTimers();
			}
			else if(GameManager.singleton.boat.GetComponent<EventCatcher>().GetCurrentFishingArea().tag == "FlatfishArea")
			{
				PutFlatFishInBasket(1,true);
                GameManager.singleton.flatFishCaught = true;
                guidance.resetGuidanceTimers();
			}

		} else {
			noCatch();
		}

		try
		{
			GameManager.singleton.boat.GetComponent<EventCatcher>().GetCurrentFishingArea().GetComponent<FishContent>().DestroyEmptyArea();
		}
		catch{}

	}
	public void BasketAniDone(){
		bool basketFull = true;
		
		if (basketFull)
		{
			trapFull();

			Debug.Log("putting fish in basket");
			//PutEelInBasket();
			PutEelInBasket(EelsInTrap,true);
						
			Debug.Log("putting fish in basket");
			//PutEelInBasket();
			PutFlatFishInBasket(FlatFishInTrap,true);
			
			Debug.Log("Trap Full");
			basketFull = false;

            guidance.resetGuidanceTimers();
            GameManager.singleton.eelTrapEmptied = true;

            GameManager.singleton.canMove = true;
            Debug.Log("i can move again");
            partnerSpeech.PartnerSaysSomething(partnerSpeech.afterEmptyingEeltrap);
        } else {
			trapEmpty();
			Debug.Log("Trap Empty");
		}

	}
	public void PutFlatFishInBasket(float amount, bool talk)
	{
		GameObject currentFish=null;
		/*if(!GameManager.singleton.Islinear && GameManager.singleton.tribeBoat.GetComponent<TribeController>().GetFollowPlayer())
		{
			GetComponent<PartnerSpeech>().PartnerSaysSomething(GetComponent<PartnerSpeech>().MeetingTriibeStoleFish);
		}*/
		if(!GameManager.singleton.Islinear && firstTimeFlatfish && !GameManager.singleton.tribeBoat.GetComponent<TribeController>().GetFollowPlayer())
		{
			firstTimeFlatfish = false;
			//GetComponent<PartnerSpeech>().PartnerSaysSomething(GetComponent<PartnerSpeech>().FirstTimeFlatFish);
		}
		//instatiate a fish in the boay
		for (int i = 1; i < amount+1; i++)
		{

			Transform[] trans = GameObject.FindGameObjectWithTag("basket").GetComponentsInChildren<Transform>(true);
			foreach (Transform t in trans) {
				if (t.gameObject.name == "flatfish_Caught_0"+i) 
				{
					currentFish = t.gameObject;
					currentFish.SetActive(true);
                    basketSound.Play();
					Debug.Log(currentFish);
		            GameManager.singleton.AddFlatFish(currentFish);
				}
			}			
		}
		//GameManager.singleton.AddFlatFish(mostRecentFish);
		//mostRecentFish = Instantiate(GameManager.singleton.flatFish,boat.transform.position+ new Vector3(0,1,0), boat.transform.rotation, boat.transform);
		
		
	}
	public void PutTorskInBasket(float amount)
	{
		amount = amount + GameManager.singleton.currentTorskAmount;
		GameObject currentFish=null;
		
		if(!GameManager.singleton.Islinear && firstTimeCod  && !GameManager.singleton.tribeBoat.GetComponent<TribeController>().GetFollowPlayer())
		{
			firstTimeCod = false;
			//GetComponent<PartnerSpeech>().PartnerSaysSomething(GetComponent<PartnerSpeech>().FirstTimeCod);
		}
		
		//instatiate a fish in the boay
		for (int i = 1; i < amount+1; i++)
		{

			Transform[] trans = GameObject.FindGameObjectWithTag("basket").GetComponentsInChildren<Transform>(true);
			foreach (Transform t in trans) {
				if (t.gameObject.name == "torsk_Caught_0"+i) 
				{
					currentFish = t.gameObject;
					currentFish.SetActive(true);
                    basketSound.Play();
                    Debug.Log(currentFish);
					Debug.Log("add torsk to basket");
		            GameManager.singleton.AddTorsk(currentFish);
				}
			}
			
		}
		//GameManager.singleton.AddTorsk(mostRecentFish);
		//mostRecentFish = Instantiate(GameManager.singleton.torsk,boat.transform.position+ new Vector3(0,1,0), boat.transform.rotation, boat.transform);
	}
	public void PutEelInBasket(int amount, bool talk)
	{
		amount = amount + GameManager.singleton.currentEelAmount;
		GameObject currentFish=null;
		/*if(!GameManager.singleton.Islinear && GameManager.singleton.tribeBoat.GetComponent<TribeController>().GetFollowPlayer())
		{
			GetComponent<PartnerSpeech>().PartnerSaysSomething(GetComponent<PartnerSpeech>().MeetingTriibeStoleFish);
		}*/
		if(!GameManager.singleton.Islinear && firstTimeEel  && !GameManager.singleton.tribeBoat.GetComponent<TribeController>().GetFollowPlayer())
		{
			firstTimeEel = false;
			//GetComponent<PartnerSpeech>().PartnerSaysSomething(GetComponent<PartnerSpeech>().FirstTimeEel);
		}
		
		//instatiate a fish in the boay
		for (int i = 1; i < amount+1; i++)
		{
			
			Transform[] trans = GameObject.FindGameObjectWithTag("basket").GetComponentsInChildren<Transform>(true);
			foreach (Transform t in trans) {
				if (t.gameObject.name == "eel_Caught_0"+i) 
				{
					currentFish = t.gameObject;
					currentFish.SetActive(true);
                    basketSound.Play();
                    Debug.Log(currentFish);
					Debug.Log("add eel to basket");
		            GameManager.singleton.AddEel(currentFish);
				}
			}
		}
		//mostRecentFish = Instantiate(GameManager.singleton.eel,boat.transform.position + new Vector3(0,1,0), boat.transform.rotation, boat.transform);
		
	}
	void ResetToolOnGuide()
	{
		GameManager.singleton.paddle.SetActive(true);
		GameManager.singleton.aniEelIron.SetActive(false);
		GameManager.singleton.aniTorch.SetActive(false);
	}


}
