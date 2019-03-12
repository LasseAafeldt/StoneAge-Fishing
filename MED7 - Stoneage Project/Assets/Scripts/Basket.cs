using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour {

	bool isDestroyed;



	// Use this for initialization
	void Start () {
		isDestroyed = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DestroyBasket()
	{
		isDestroyed=true;
		Debug.Log("basket destroyed");

		GetComponent<Collider>().enabled = false;

		//switch or alter the basket model to be destroyed

		//make the other tribe follow you
	}
}
