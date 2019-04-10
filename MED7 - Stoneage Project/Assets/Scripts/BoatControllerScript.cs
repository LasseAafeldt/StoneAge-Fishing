using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatControllerScript : MonoBehaviour
{

    public static string currentlyInRegion = "No Region";

    public float speed = 15f;
    public float rotationSpeed = 0.1f;
    public float rotationDeadzone = 5f;
    public Collider[] ignoreCollision;

    bool voiceLineReady = true;

    float verticalInput;
    float horizontalInput;
    float steerFactor;

    private bool outOfBounds = false;


    // Update is called once per frame
    void Update()
    {
        if (GameManager.singleton.canMove)
        {
            BoatMovement();
        }
    }

    public void BoatMovement()
    {
        if (Input.GetButtonDown("Fire1") && !outOfBounds && !GameManager.singleton.pointingAtInteractable && GameManager.singleton.canMove)
        {
            GameManager.singleton.paddle.GetComponent<AudioSource>().Play();
            GameManager.singleton.partner.GetComponent<PartnerAnimator>().paddleAnimation(true);
        }
        else if (Input.GetButtonUp("Fire1") && !outOfBounds)
        {
            GameManager.singleton.paddle.GetComponent<AudioSource>().Stop();
            GameManager.singleton.partner.GetComponent<PartnerAnimator>().paddleAnimation(false);
        }

        if (Input.GetButton("Fire1") && !outOfBounds)
        {
            float sinusoid = (Mathf.Sin(Time.time) + 1) / 2;
            if (sinusoid < 0.2f)
            {
                sinusoid = 0.2f;
            }

            #region Linear stuff
            /*
            if (Vector3.Distance(GameManager.singleton.currentPillar.transform.position, transform.position) > 150)
			{
				Debug.Log("FORKERT VEJ!");
				float wrongWayAngle = Vector3.SignedAngle(transform.forward, new Vector3(GameManager.singleton.currentPillar.transform.position.x, 0, GameManager.singleton.currentPillar.transform.position.z) - transform.position, Vector3.up);
				if(wrongWayAngle > 90)
				{
					// Right
					GameManager.singleton.partner.GetComponent<PartnerAnimator>().paddleAnimation(false);
					GameManager.singleton.partner.GetComponent<PartnerAnimator>().wrongWay(true);
					GameManager.singleton.partner.GetComponent<PartnerAnimator>().pointRight(true);
					GameManager.singleton.partner.GetComponent<PartnerAnimator>().pointLeft(false);
					if(voiceLineReady)
					{
						voiceLineReady = false;
						GameManager.singleton.partner.GetComponent<PartnerSpeech>().PartnerSaysSomething(
							GameManager.singleton.partner.GetComponent<PartnerSpeech>().ThisWay1,false);
					}

				} else if (wrongWayAngle < -90) {
					// Left
					GameManager.singleton.partner.GetComponent<PartnerAnimator>().paddleAnimation(false);
					GameManager.singleton.partner.GetComponent<PartnerAnimator>().wrongWay(true);
					GameManager.singleton.partner.GetComponent<PartnerAnimator>().pointLeft(true);
					GameManager.singleton.partner.GetComponent<PartnerAnimator>().pointRight(false);
					if(voiceLineReady)
					{
						voiceLineReady = false;
						GameManager.singleton.partner.GetComponent<PartnerSpeech>().PartnerSaysSomething(
							GameManager.singleton.partner.GetComponent<PartnerSpeech>().ThisWay2,false);
					}
				} else {
					voiceLineReady = true;
					GameManager.singleton.partner.GetComponent<PartnerAnimator>().paddleAnimation(true);
					GameManager.singleton.partner.GetComponent<PartnerAnimator>().wrongWay(false);
					GameManager.singleton.partner.GetComponent<PartnerAnimator>().pointLeft(false);
					GameManager.singleton.partner.GetComponent<PartnerAnimator>().pointRight(false);
				}
			
			}
            */
            #endregion

            if (!GameManager.singleton.pointingAtInteractable)
            {


                if (GameManager.singleton.partner.GetComponent<PartnerAnimator>().anim.GetBool("isRowing"))
                {
                    //Debug.Log("Im going forwards");
                    GetComponent<Rigidbody>().AddForce(transform.forward * speed * sinusoid * Time.deltaTime);
                }

                float rotationAngle = Vector3.Angle(transform.forward, Camera.main.transform.forward);

                if (rotationAngle > rotationDeadzone)
                {

                    Vector3 newDir = Vector3.RotateTowards(transform.forward, new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z), rotationAngle, 0.0f);

                    // calculate the Quaternion for the rotation
                    Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newDir), rotationSpeed * Time.deltaTime);
                    //Quaternion rot = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(newDir), rotationSpeed * Time.deltaTime);

                    //Apply the rotation 
                    transform.rotation = rot;

                }
            }
            //Debug.Log(Vector3.Angle(transform.forward, Camera.main.transform.forward));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Regions"))
            return;
        //Debug.Log("layer should be regions, and it is = " + LayerMask.LayerToName(other.gameObject.layer));
        setCurrentRegion(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Regions"))
            return;
        //Debug.Log("layer should be regions, and it is = " + other.gameObject.layer);
        resetCurrentRegion();
    }

    void setCurrentRegion(GameObject region)
    {
        currentlyInRegion = region.name;
        Debug.Log("Current region is now: " + currentlyInRegion);
    }

    public string getCurrentRegion()
    {
        return currentlyInRegion;
    }

    void resetCurrentRegion()
    {
        //set curentregion to no region
        currentlyInRegion = "No Region";
        Debug.Log("Current region is now: " + currentlyInRegion);
        //if multiple colliders is use to make one region, then check if other region colliders overlap with current position
    }

    #region More force turn 
    /*
    IEnumerator forceRotate()
		{
			// The step size is equal to speed times frame time.
			float rotStep = rotationSpeed * Time.deltaTime;

			while (transform.forward != (new Vector3(0,transform.position.y,0) - transform.position).normalized)
			{
				Vector3 newDir = Vector3.RotateTowards(transform.forward, (new Vector3(0,transform.position.y,0) - transform.position), rotStep, 0.0f);
					
				// calculate the Quaternion for the rotation
				Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newDir), rotationSpeed * Time.deltaTime);
				
				//Apply the rotation 
				transform.rotation = rot;

				Debug.Log(transform.forward + " ---- " + (new Vector3(0,transform.position.y,0) - transform.position).normalized);

				yield return null;
			}
			
			yield return null;
		}

	IEnumerator forceMove()
    {
		// The step size is equal to speed times frame time.
        float step = speed * Time.deltaTime;
		float rotStep = rotationSpeed * Time.deltaTime;
		float dist = Vector3.Distance(transform.position, new Vector3(0,transform.position.y,0));

		

		while (transform.position != new Vector3(0,transform.position.y,0))
		{
			// Move our position a step closer to the target.
        	transform.position = Vector3.MoveTowards(transform.position, new Vector3(0,transform.position.y,0), step);
			
			Vector3 newDir = Vector3.RotateTowards(transform.forward, (new Vector3(0,transform.position.y,0) - transform.position), rotStep, 0.0f);
				
			// calculate the Quaternion for the rotation
			Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newDir), rotationSpeed * Time.deltaTime);
			
			//Apply the rotation 
			transform.rotation = rot;

			if((Vector3.Distance(transform.position, new Vector3(0,transform.position.y,0)) < (dist - 40)))
			{
				StopAllCoroutines();
			}
			//Debug.Log(dist + " ---- " + (new Vector3(0,transform.position.y,0) - transform.position).normalized);
			
			yield return null;
		}
    }
    */
    #endregion
}