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
        ps = GameManager.singleton.partner.GetComponent<PartnerSpeech>();
    }

    // Update is called once per frame
    void Update () {
       
    }

    private void OnTriggerEnter(Collider collision) //collider collision er sådan man kalder den. 
    {
        if (orcaHasBeenActivated)
        {
            return;
        }
        if (collision.gameObject.tag == GameManager.singleton.boat.tag)
        {
            playTimer.timeLeft += playAnimation.length + 0.5f;
            orca.SetActive(true); //gør gameobjected aktivt
            GameManager.singleton.canMove = false;
            //reset the paddle animation
            GameManager.singleton.partner.GetComponent<PartnerAnimator>().paddleAnimation(false);

            //Play "se en spækhugger" sound
            ps.PartnerSaysSomething(ps.OrcaAppears);

            StartCoroutine(waitForOrcaAnimation(playAnimation));

            orcaHasBeenActivated = true;
        }
    }
    IEnumerator waitForOrcaAnimation(AnimationClip clip)
    {
        yield return new WaitForSeconds(clip.length);
        orca.SetActive(false);
        GameManager.singleton.canMove = true;
    }

}
