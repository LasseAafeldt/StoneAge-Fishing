﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartnerAnimator : MonoBehaviour {


    [HideInInspector]
	public Animator anim;

    public delegate void FishPutInBasket();
    public static event FishPutInBasket FishPutInBasketEvent;

    public PartnerSpeech partnerSpeech;
    public GuidanceSounds guidance;
    public Transform eeltrap;
    public float eelTrapRange = 10f;
    public AudioSource basketSound;

    private FishStoreAmount eelTrapFishStorage;
    private int _eelsInTrap;
    private int _flatFishInTrap;

	// Use this for initialization
	void Start () {

        anim = GetComponent<Animator>();
        eelTrapFishStorage = eeltrap.GetComponent<FishStoreAmount>();
        _eelsInTrap = eelTrapFishStorage.EelAmount;
        _flatFishInTrap = eelTrapFishStorage.FlatfishlAmount;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	public void HookAnimation()
	{
		anim.SetTrigger("hookFishing");
        GameManager.singleton.canMove = false;
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
	}

	public void eelCaught()
	{
		anim.SetTrigger("eelCaught");
        GameManager.singleton.canMove = true;
    }

	public void noCatch()
	{
		anim.SetTrigger("noCatch");
        GameManager.singleton.canMove = true;
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
        GameManager.singleton.paddle.SetActive(state);
	}

    //NOTE here
	public void HookAniDone(){
		GameManager.singleton.hook.GetComponent<SelectTool>().ShowTool();
		GameManager.singleton.paddle.SetActive(true);
		GameManager.singleton.partner.transform.position = GameManager.singleton.partner.transform.position + 0.75f*transform.up;
		if(GameManager.singleton.boat.GetComponent<EventCatcher>().GetCanFish())
		{
			codCaught();
			PutTorskInBasket(1);

            //tosk area has now been used once
            GameManager.singleton.TorskCaught = true;

            //maybe do reset of guidance timer here????
            guidance.ResetGuidanceTimers();
        } else {
			noCatch();
		}

		//check if there is more fish in the area
		try
		{
			//GameManager.singleton.boat.GetComponent<EventCatcher>().GetCurrentFishingArea().GetComponent<FishContent>().DestroyEmptyArea();
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
		if(GameManager.singleton.boat.GetComponent<EventCatcher>().GetCanFish())
		{
			eelCaught();
			if(GameManager.singleton.boat.GetComponent<EventCatcher>().GetCurrentFishingArea().tag == "EelArea")
			{
				
				PutEelInBasket(1);
                GameManager.singleton.eelCaught = true;
                guidance.ResetGuidanceTimers();
			}
			else if(GameManager.singleton.boat.GetComponent<EventCatcher>().GetCurrentFishingArea().tag == "FlatfishArea")
			{
				PutFlatFishInBasket(1);
                GameManager.singleton.flatFishCaught = true;
                guidance.ResetGuidanceTimers();
			}

		} else {
			noCatch();
            /*//temp fix while using flaring animotion for emptying 
             * 
             * trap
            float dist = Vector3.Distance(transform.position, eeltrap.position);
            if(dist < eelTrapRange)
            {
                trapFull();
            }*/
		}

		try
		{
			//GameManager.singleton.boat.GetComponent<EventCatcher>().GetCurrentFishingArea().GetComponent<FishContent>().DestroyEmptyArea();
		}
		catch{}

	}
	public void BasketAniDone(){
		bool basketFull = true;

		if (basketFull)
		{
			trapFull();

			PutEelInBasket(_eelsInTrap);
						
			PutFlatFishInBasket(_flatFishInTrap);
			
			basketFull = false;

            guidance.ResetGuidanceTimers();
            GameManager.singleton.eelTrapEmptied = true;

            GameManager.singleton.canMove = true;

        } else {
			trapEmpty();
		}

	}
	public void PutFlatFishInBasket(int amount)
	{
        int fishToInstanciate = amount + GameManager.singleton.getFlatfishAmount();
        GameManager.singleton.AddFlatFish(amount);

		//instatiate a fish in the boay
        GameObject currentFish=null;
        Transform[] trans = GameObject.FindGameObjectWithTag("basket").GetComponentsInChildren<Transform>(true);
		for (int i = 1; i <= fishToInstanciate; i++)
		{
			foreach (Transform t in trans) {
				if (t.gameObject.name == "flatfish_Caught_0"+i) 
				{
					currentFish = t.gameObject;
					currentFish.SetActive(true);
                }
			}
        }
        basketSound.Play();
        InvokeFishPutInBasketEvent();
    }
	public void PutTorskInBasket(int amount)
	{
        int fishToInstanciate = amount + GameManager.singleton.getTorskAmount();
        GameManager.singleton.AddTorsk(amount);

        //instatiate a fish in the boat
        GameObject currentFish = null;
        Transform[] trans = GameObject.FindGameObjectWithTag("basket").GetComponentsInChildren<Transform>(true);
		for (int i = 1; i <= fishToInstanciate; i++)
		{
			foreach (Transform t in trans) {
				if (t.gameObject.name == "torsk_Caught_0"+i) 
				{
					currentFish = t.gameObject;
                    //loop through all torsk in basket and make amount visible
					currentFish.SetActive(true);
                }
			}
        }
        basketSound.Play();
        InvokeFishPutInBasketEvent();
    }
	public void PutEelInBasket(int amount)
	{
        int fishToInstanciate = amount + GameManager.singleton.getEelAmount();
        GameManager.singleton.AddEel(amount);
     

        //instatiate a fish in the boat
        GameObject currentFish = null;
        Transform[] trans = GameObject.FindGameObjectWithTag("basket").GetComponentsInChildren<Transform>(true);
		for (int i = 1; i <= fishToInstanciate; i++)
		{
			foreach (Transform t in trans) {
				if (t.gameObject.name == "eel_Caught_0"+i) 
				{
					currentFish = t.gameObject;
					currentFish.SetActive(true);
                }
			}		    
		}
        basketSound.Play();
        InvokeFishPutInBasketEvent();
    }
	void ResetToolOnGuide()
	{
		GameManager.singleton.paddle.SetActive(true);
		GameManager.singleton.aniEelIron.SetActive(false);
		GameManager.singleton.aniTorch.SetActive(false);
	}

    private static void InvokeFishPutInBasketEvent()
    {        
        if (FishPutInBasketEvent != null)
        {
            FishPutInBasketEvent.Invoke();
        }
    }

}
