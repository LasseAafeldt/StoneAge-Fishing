using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : MonoBehaviour {

    public List<Transform> points = new List<Transform>();

	// Use this for initialization
	void Start () {
		transform.position = points[0].position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
