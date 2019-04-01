using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelicanTrigger : MonoBehaviour {

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("hello pelican");
    }
}
