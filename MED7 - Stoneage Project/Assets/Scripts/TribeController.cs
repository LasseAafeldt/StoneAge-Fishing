using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribeController : MonoBehaviour {

	public float speed = 0.2f;
	public float rotationSpeed = 160f;

	public GameObject pillar;
	Vector3 newPos;
	float verticalInput;
	float horizontalInput;
	float steerFactor;
	
	GameObject playerBoat;
	Vector3 playerPos;

	Vector3 leftSideOfBoat;
	Vector3 rightSideOfBoat;
	float distance2left;
	float distance2right;


	Vector3 steerDirection = new Vector3(0,0,0);
	bool followPlayer = false;
	public bool toPosition = false;

	GameObject tribeTrigger;

	void Start()
	{
		playerBoat = GameManager.singleton.boat;
		tribeTrigger = GameObject.FindGameObjectWithTag("tribeTrigger");
		newPos=pillar.transform.position;
	}

	// Update is called once per frame
	void Update () {
		//Movement();
		//TestSteer();
		playerPos = playerBoat.transform.position;

		leftSideOfBoat  = new Vector3(playerPos.x-5,playerPos.y,playerPos.z);
		rightSideOfBoat = new Vector3(playerPos.x+5,playerPos.y,playerPos.z);

		//GetComponentInChildren<Transform>().rotation=playerBoat.transform.rotation;

		distance2left=Vector3.Distance(leftSideOfBoat, transform.position);
		distance2left=Vector3.Distance(rightSideOfBoat, transform.position);

		if(toPosition)
		{
			steerDirection = newPos - transform.position;
		}
		else if(distance2left<distance2right)
		{
			steerDirection = leftSideOfBoat - transform.position;
		}
		else
		{
			steerDirection = rightSideOfBoat - transform.position;
		}
		//Debug.DrawRay(transform.position,steerDirection, Color.red, 10);
		BoatMovement();
	}

	void Movement () {
		verticalInput = Input.GetAxis("Vertical");
		if(Input.GetAxis("Vertical") != 0)
		{
			GetComponent<Rigidbody>().AddForce(Input.GetAxis("Vertical") * transform.forward * speed * Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup * 1.0f)));
		}
	}

	void TestSteer () {
		horizontalInput = Input.GetAxis("Horizontal");
		steerFactor = Mathf.Lerp(steerFactor, horizontalInput, Time.deltaTime);
		transform.Rotate(0.0f, steerFactor, 0.0f);
	}

	public void BoatMovement () {

		if(followPlayer || toPosition)
		{
			//Debug.Log(Vector3.Distance(pillar.transform.position, transform.position));
			if(Vector3.Distance(new Vector3(newPos.x, 0, newPos.z), transform.position)<15)
			{
				toPosition = false;
			}

			GetComponent<Rigidbody>().AddForce(transform.forward * speed * Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup * 1.0f)));

			if (Vector3.Angle(steerDirection, Camera.main.transform.forward) > 10)
			//if (Vector3.Angle(transform.forward, Camera.main.transform.forward) > 10)
			//if (Vector3.Angle(transform.forward, steerDirection) > 10)
			{
				// The step size is equal to speed times frame time.
				float step = rotationSpeed * Time.deltaTime;

				//Vector3 newDir = Vector3.RotateTowards(transform.forward, new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z), step, 0.0f);
				//Vector3 newDir = Vector3.RotateTowards(steerDirection, new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z), step, 0.0f);
				Vector3 newDir = Vector3.RotateTowards(transform.forward, new Vector3(steerDirection.x, 0, steerDirection.z), step, 0.0f);
				
				// calculate the Quaternion for the rotation
				Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newDir), rotationSpeed * Time.deltaTime);
				
				//Apply the rotation 
				transform.rotation = rot;
			}
			//Debug.Log(Vector3.Angle(transform.forward, Camera.main.transform.forward));
		}

	}

	public void GetInPosition()
	{
		toPosition=true;
	}
		public void GetInPosition(Vector3 newPos)
	{
		this.newPos = newPos;
		toPosition=true;
	}

	public bool GetFollowPlayer()
	{
		return followPlayer;
	}

	public void SetFollowPlayer(bool input)
	{
		Debug.Log("setting follow player to "+input);
		followPlayer = input;
		tribeTrigger.GetComponent<Collider>().enabled = input;
		if(input)
		{
			GameManager.singleton.tradingObject.GetComponent<Collider>().enabled=false;
		}
	}
}