using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class GuidanceSounds : MonoBehaviour {
    public PartnerSpeech voiceLines;
    public bool GuidanceIsOn = true;
    public float triggerRadius = 150;

    [Header("Trigger areas")]
    public Collider Torsk;
    public Collider eel;
    public Collider eelTrap;
    public Collider start;

    bool torskHasPlayed = false;
    bool eelHasPlayed = false;
    bool eeltrapHasPlayed = false;
    bool startHasPlayed = false;

    private void Start()
    {
        GetComponent<SphereCollider>().radius = triggerRadius;
        GetComponent<SphereCollider>().isTrigger = true;
        GetComponent<Rigidbody>().useGravity = false;
    }

    // trigger bakke sound when closest to torsk fishing
    // trugger fugle sound when closest to ål areas
    // trigger Ruse sound when closest to eel trap
    // trigger start sound when ..................

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == Torsk.name || other.name == eel.name || other.name == eelTrap.name || other.name == start.name)
        {
            Debug.Log(other.name);
            if(other.name == Torsk.name && !torskHasPlayed)
            {
                voiceLines.PartnerSaysSomething(voiceLines.Bakke);
                torskHasPlayed = true;
            }
            if(other.name == eel.name && !eelHasPlayed)
            {
                voiceLines.PartnerSaysSomething(voiceLines.Fugle);
                eelHasPlayed = true;
            }
            if(other.name == eelTrap.name && !eeltrapHasPlayed)
            {
                voiceLines.PartnerSaysSomething(voiceLines.Ruse);
                eeltrapHasPlayed = true;
            }
            if(other.name == start.name && !startHasPlayed)
            {
                voiceLines.PartnerSaysSomething(voiceLines.Starting);
                startHasPlayed = true;
            }
        }
    }
}
