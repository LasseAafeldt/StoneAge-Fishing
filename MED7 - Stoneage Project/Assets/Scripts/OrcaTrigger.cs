using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcaTrigger : MonoBehaviour {
    //public Collider collider;
    public GameObject orca; //makes sure the model is active
    public AnimationClip playAnimation;
    PartnerSpeech ps;
    bool orcaHasBeenActivated = false;

	// Use this for initialization
	void Start () {
        orcaHasBeenActivated = false;
        ps = GameManager.singleton.partner.GetComponent<PartnerSpeech>();
    }

    // Update is called once per frame
    void Update () {
       
    }

    private void OnTriggerEnter(Collider collision) //collider collision er sådan man kalder den. 
    {
        //Debug.Log(collision.gameObject.name);
        if (orcaHasBeenActivated || collision.tag != "boat")
        {
            return;
        }
        Debug.Log("Collider is tag boat");
        playTimer.timeLeft += playAnimation.length + 0.5f;
        orca.SetActive(true); //gør gameobjected aktivt
        GameManager.singleton.canMove = false;
        Debug.Log("I can't move anymore");
        //reset the paddle animation
        GameManager.singleton.partner.GetComponent<PartnerAnimator>().paddleAnimation(false);

        //Play "se en spækhugger" sound
        ps.PartnerSaysSomething(ps.OrcaAppears);

        StartCoroutine(waitForOrcaSpeech(ps.OrcaAppears.length)); //now wait for sound instead of animation to finish
    
        orcaHasBeenActivated = true;
    }
    IEnumerator waitForOrcaSpeech(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        GameManager.singleton.canMove = true;
        Debug.Log("I can move now");
        StartCoroutine(waitForOrcaAnimation(playAnimation.length));
    }
    IEnumerator waitForOrcaAnimation(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        orca.SetActive(false);
    }
}
