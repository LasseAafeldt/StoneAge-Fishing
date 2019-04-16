using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealTrigger : MonoBehaviour {
    PartnerSpeech ps;
    bool hasPlayed = false;

    private void Start()
    {
        hasPlayed = false;
        ps = GameManager.singleton.partner.GetComponent<PartnerSpeech>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "boat")
        {
            return;
        }

        if (!hasPlayed)
        {
            ps.PartnerSaysSomething(ps.SealAppearsEmergent);
            hasPlayed = true;
        }
    }
}
