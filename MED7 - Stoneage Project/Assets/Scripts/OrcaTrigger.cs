using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcaTrigger : MonoBehaviour {
    //public Collider collider;
    public GameObject orca; //makes sure the model is active
    public AnimationClip playAnimation;
    PartnerSpeech ps;

	// Use this for initialization
	void Start () {
        ps = GameManager.singleton.partner.GetComponent<PartnerSpeech>();
    }

    // Update is called once per frame
    void Update () {
       
    }

    private void OnTriggerEnter(Collider collision) //collider collision er sådan man kalder den. 
    {
        if (collision.gameObject.tag == GameManager.singleton.boat.tag)
        {
            orca.SetActive(true); //gør gameobjected aktivt
            GameManager.singleton.canMove = false;

            //Play "se en spækhugger" sound
            ps.PartnerSaysSomething(ps.OrcaAppears);

            StartCoroutine(waitForOrcaAnimation(playAnimation));
        }
    }
    IEnumerator waitForOrcaAnimation(AnimationClip clip)
    {
        yield return new WaitForSeconds(clip.length);
        orca.SetActive(false);
        GameManager.singleton.canMove = true;
    }

}
