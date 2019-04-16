using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class retunToMiddenSound : MonoBehaviour {

    Collider middenRange;
    PartnerSpeech ps;
    bool isHome = false;
    bool canPlaySound = false;

    private void Start()
    {
        middenRange = GetComponent<SphereCollider>();
        ps = GameManager.singleton.partner.GetComponent<PartnerSpeech>();
        StartCoroutine(waitForCanPlay());
        canPlaySound = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name != "Starting area" || !canPlaySound)
        {
            return;
        }
        Debug.Log("other collider is = " + other.name);

        if (!isHome)
        {
            ps.PartnerSaysSomething(ps.HomeAgain);
            isHome = true;
            StartCoroutine(timeTillHomeAgain());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name != "Starting area")
        {
            return;
        }
        isHome = false;
    }

    IEnumerator timeTillHomeAgain()
    {
        yield return new WaitForSeconds(10f);
        isHome = false;
    }

    IEnumerator waitForCanPlay()
    {
        yield return new WaitForSeconds(20f);
        canPlaySound = true;
        Debug.Log("Can play sound is now true....................");
    }
}
