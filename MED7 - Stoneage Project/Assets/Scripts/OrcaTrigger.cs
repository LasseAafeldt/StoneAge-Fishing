using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcaTrigger : MonoBehaviour {
    public Collider collider;
    public GameObject orca;
    public AnimationClip playAnimation;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
       
    }

    private void OnTriggerEnter(Collider collision)
    {
        orca.SetActive(true);
        StartCoroutine(waitForOrcaAnimation(playAnimation));
        Debug.Log("hello world");
    }
    IEnumerator waitForOrcaAnimation(AnimationClip clip)
    {
        yield return new WaitForSeconds(clip.length);
        orca.SetActive(false);
    }

}
