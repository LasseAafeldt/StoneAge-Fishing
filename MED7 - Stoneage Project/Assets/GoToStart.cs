using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToStart : MonoBehaviour {

	public float totalTime =10;
    float tempTime;
	// Use this for initialization
	void Start () {
        tempTime = totalTime;
	}
	
	// Update is called once per frame
	void Update () {
		tempTime -= Time.deltaTime;

		if(tempTime < 0 && Input.GetButtonDown("Fire1"))
		{
			Debug.Log("go to starrt");
            //LogMaster.shouldBeLogging = true; this enables it too early
			Destroy(GameObject.Find("Game Manager"));
			SceneManager.LoadScene("start", LoadSceneMode.Single);
		}
	}
}
