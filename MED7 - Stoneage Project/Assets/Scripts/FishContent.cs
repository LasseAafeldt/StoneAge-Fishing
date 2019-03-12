using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishContent : MonoBehaviour {

	public List<GameObject> fish = new List<GameObject>();


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RemoveFish()
	{
		if(fish.Count >0)
		{
			//Debug.Break();
			Debug.Log("number of fish before " +fish.Count);
			Destroy(fish[0].gameObject);
			fish.RemoveAt(0);
			Debug.Log("number of fish after " +fish.Count);

		}

	}
	public void DestroyEmptyArea()
	{
		if(fish.Count == 0)
		{
			Debug.Log("destroying the area");
			Destroy(gameObject);
			GameManager.singleton.boat.GetComponent<EventCatcher>().ExitArea();
		}
	}

}
