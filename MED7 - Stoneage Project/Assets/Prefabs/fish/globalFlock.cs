using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalFlock : MonoBehaviour {

    public Transform parent; 
    public GameObject fishPrefab;
    //public GameObject goalPrefab;
    public static int schoolSize = 5;

    static int numberofFish = 15;
    public static GameObject[] allFish = new GameObject[numberofFish];

    public static Vector3 goalPos = Vector3.zero;
    private Vector3 parentPos;
	// Use this for initialization
	void Start () {
        parentPos = parent.position;
		for(int i = 0; i < numberofFish; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-schoolSize, schoolSize)+parentPos.x, Random.Range(-schoolSize, schoolSize)+parentPos.y, Random.Range(-schoolSize, schoolSize)+parentPos.z);
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity, parent);
        }
	}
	
	// Update is called once per frame
	void Update () {
        goalPos = new Vector3(Random.Range(-schoolSize, schoolSize)+parentPos.x, Random.Range(-schoolSize, schoolSize)+parentPos.y , Random.Range(-schoolSize, schoolSize)+parentPos.z);
        if (goalPos.y > 0)
        {
             goalPos.y = transform.position.y;
        }
        //goalPrefab.transform.position = goalPos;
	}
}
