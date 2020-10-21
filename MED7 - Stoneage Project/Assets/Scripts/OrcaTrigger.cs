using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcaTrigger : MonoBehaviour {
    public GameObject orca;
    public AnimationClip playAnimation;
    PartnerSpeech ps;
    bool orcaHasBeenActivated = false;

	void Start () {
        orcaHasBeenActivated = false;
        ps = GameManager.singleton.partner.GetComponent<PartnerSpeech>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (orcaHasBeenActivated || collision.tag != "boat")
        {
            return;
        }
        playTimer.timeLeft += playAnimation.length + 0.5f;
        orca.SetActive(true);
        GameManager.singleton.canMove = false;
        //reset the paddle animation
        GameManager.singleton.partner.GetComponent<PartnerAnimator>().paddleAnimation(false);

        //Play "se en spækhugger" sound
        ps.PartnerSaysSomething(ps.OrcaAppears);

        //now wait for sound instead of animation to finish
        StartCoroutine(waitForOrcaSpeech(ps.OrcaAppears.length)); 
    
        orcaHasBeenActivated = true;
    }
    IEnumerator waitForOrcaSpeech(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        GameManager.singleton.canMove = true;
        StartCoroutine(waitForOrcaAnimation(playAnimation.length));
    }
    IEnumerator waitForOrcaAnimation(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        orca.SetActive(false);
    }
}
