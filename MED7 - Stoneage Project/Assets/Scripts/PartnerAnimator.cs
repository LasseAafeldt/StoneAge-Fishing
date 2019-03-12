using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartnerAnimator : MonoBehaviour {



	public Animator anim;

	GameObject boat;

	GameObject mostRecentFish;

	bool firstTimeEel =true;
	bool firstTimeCod =true;
	bool firstTimeFlatfish =true;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		boat = GameManager.singleton.spawnPoint;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void HookAnimation()
	{
		anim.SetTrigger("hookFishing");
		GameManager.singleton.paddle.SetActive(false);
		GameManager.singleton.partner.transform.position = GameManager.singleton.partner.transform.position - 0.75f*transform.up;
	}
	public void EelironAnimation()
	{
		anim.SetTrigger("eelIronFishing");
		GameManager.singleton.paddle.SetActive(false);
		GameManager.singleton.aniEelIron.SetActive(true);
		GameManager.singleton.aniTorch.SetActive(true);
	}

	public void BasketAnimation()
	{
		anim.SetTrigger("basketFishing");
	}

	public void trapEmpty()
	{
		if(!GameManager.singleton.Islinear)
		{
			if(GameManager.singleton.tribeBoat.GetComponent<TribeController>().GetFollowPlayer())
			{
				GetComponent<PartnerSpeech>().PartnerSaysSomething(GetComponent<PartnerSpeech>().MeetingTriibeStoleFish);
			}
			else
			{
				GetComponent<PartnerSpeech>().PartnerSaysSomething(GetComponent<PartnerSpeech>().CheckBasketNoFish);
			}
		}
		anim.SetTrigger("trapEmpty");
	}

	public void trapFull()
	{
		if(!GameManager.singleton.Islinear)
		{
			if(GameManager.singleton.tribeBoat.GetComponent<TribeController>().GetFollowPlayer())
			{
				GetComponent<PartnerSpeech>().PartnerSaysSomething(GetComponent<PartnerSpeech>().MeetingTriibeStoleFish);
			}
			else
			{
				GetComponent<PartnerSpeech>().PartnerSaysSomething(GetComponent<PartnerSpeech>().CheckBasketFish);
			}

		}
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
	}

	public void eelCaught()
	{
		anim.SetTrigger("eelCaught");
	}

	public void noCatch()
	{
		anim.SetTrigger("noCatch");

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
			}
			else if(GameManager.singleton.boat.GetComponent<EventCatcher>().GetCurrentFishingArea().tag == "FlatfishArea")
			{
				PutFlatFishInBasket(1,true);
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
			PutEelInBasket(4,true);
			PutEelInBasket(4,false);
			PutEelInBasket(4,false);
			PutEelInBasket(4,false);
			
			Debug.Log("putting fish in basket");
			//PutEelInBasket();
			PutFlatFishInBasket(2,true);
			PutFlatFishInBasket(2,false);
			
			if(GameManager.singleton.Islinear)
			{
				GetComponent<PartnerSpeech>().PartnerSaysSomething(GetComponent<PartnerSpeech>().AfterEmptyBasket, "FANG 3 TORSK");
			}
			Debug.Log("Trap Full");
			basketFull = false;
		} else {
			trapEmpty();
			Debug.Log("Trap Empty");
		}

	}
	public void PutFlatFishInBasket(float amount, bool talk)
	{
		GameObject currentFish=null;
		if(!GameManager.singleton.Islinear && GameManager.singleton.tribeBoat.GetComponent<TribeController>().GetFollowPlayer())
		{
			GetComponent<PartnerSpeech>().PartnerSaysSomething(GetComponent<PartnerSpeech>().MeetingTriibeStoleFish);
		}
		if(!GameManager.singleton.Islinear && firstTimeFlatfish && !GameManager.singleton.tribeBoat.GetComponent<TribeController>().GetFollowPlayer())
		{
			firstTimeFlatfish = false;
			GetComponent<PartnerSpeech>().PartnerSaysSomething(GetComponent<PartnerSpeech>().FirstTimeFlatFish);
		}
		else if(!GameManager.singleton.Islinear && !firstTimeFlatfish && !GameManager.singleton.tribeBoat.GetComponent<TribeController>().GetFollowPlayer() && talk)
		{
			GetComponent<PartnerSpeech>().PartnerSaysSomething(GetComponent<PartnerSpeech>().AnotherFlatfish);
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
					Debug.Log(currentFish);

				}
			}
			
		}
		GameManager.singleton.AddFlatFish(currentFish);
		//GameManager.singleton.AddFlatFish(mostRecentFish);
		//mostRecentFish = Instantiate(GameManager.singleton.flatFish,boat.transform.position+ new Vector3(0,1,0), boat.transform.rotation, boat.transform);
		
		
	}
	public void PutTorskInBasket(float amount)
	{
		amount = amount + GameManager.singleton.currentTorskAmount;
		GameObject currentFish=null;
		if(!GameManager.singleton.Islinear && GameManager.singleton.tribeBoat.GetComponent<TribeController>().GetFollowPlayer())
		{
			GetComponent<PartnerSpeech>().PartnerSaysSomething(GetComponent<PartnerSpeech>().MeetingTriibeStoleFish);
		}
		if(!GameManager.singleton.Islinear && firstTimeCod  && !GameManager.singleton.tribeBoat.GetComponent<TribeController>().GetFollowPlayer())
		{
			firstTimeCod = false;
			GetComponent<PartnerSpeech>().PartnerSaysSomething(GetComponent<PartnerSpeech>().FirstTimeCod);
		}
		else if(!GameManager.singleton.Islinear && !firstTimeCod  && !GameManager.singleton.tribeBoat.GetComponent<TribeController>().GetFollowPlayer())
		{
			GetComponent<PartnerSpeech>().PartnerSaysSomething(GetComponent<PartnerSpeech>().AnotherCod);
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
					Debug.Log(currentFish);
					Debug.Log("add torsk to basket");

				}
			}
			
		}
		GameManager.singleton.AddTorsk(currentFish);
		//GameManager.singleton.AddTorsk(mostRecentFish);
		//mostRecentFish = Instantiate(GameManager.singleton.torsk,boat.transform.position+ new Vector3(0,1,0), boat.transform.rotation, boat.transform);
	}
	public void PutEelInBasket(int amount, bool talk)
	{
		amount = amount + GameManager.singleton.currentEelAmount;
		GameObject currentFish=null;
		if(!GameManager.singleton.Islinear && GameManager.singleton.tribeBoat.GetComponent<TribeController>().GetFollowPlayer())
		{
			GetComponent<PartnerSpeech>().PartnerSaysSomething(GetComponent<PartnerSpeech>().MeetingTriibeStoleFish);
		}
		if(!GameManager.singleton.Islinear && firstTimeEel  && !GameManager.singleton.tribeBoat.GetComponent<TribeController>().GetFollowPlayer())
		{
			firstTimeEel = false;
			GetComponent<PartnerSpeech>().PartnerSaysSomething(GetComponent<PartnerSpeech>().FirstTimeEel);
		}
		else if(!GameManager.singleton.Islinear && !firstTimeEel  && !GameManager.singleton.tribeBoat.GetComponent<TribeController>().GetFollowPlayer() && talk)
		{
			GetComponent<PartnerSpeech>().PartnerSaysSomething(GetComponent<PartnerSpeech>().AnotherEel);
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
					Debug.Log(currentFish);
					Debug.Log("add eel to basket");

				}
			}
		}
		GameManager.singleton.AddEel(currentFish);
		//mostRecentFish = Instantiate(GameManager.singleton.eel,boat.transform.position + new Vector3(0,1,0), boat.transform.rotation, boat.transform);
		
	}
	void ResetToolOnGuide()
	{
		GameManager.singleton.paddle.SetActive(true);
		GameManager.singleton.aniEelIron.SetActive(false);
		GameManager.singleton.aniTorch.SetActive(false);
	}


}
