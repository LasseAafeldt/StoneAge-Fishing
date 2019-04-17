using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playEndAudio : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameManager.singleton._audio.Play();
	}

}
