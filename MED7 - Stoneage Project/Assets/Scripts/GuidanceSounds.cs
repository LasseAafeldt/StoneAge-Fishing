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

    public float timeSinceLastGuidance;
    public float guidanceTimerThreshold = 40f;
    public float areaTimer;
    public float areaTimerThreshold = 20f;

    string inArea = "none";
    string previousArea = "different none";

    bool torskHasPlayed = false;
    bool eelHasPlayed = false;
    bool eeltrapHasPlayed = false;
    bool startHasPlayed = false;

    PartnerSpeech partnerSpeech;
    Transform playerPos;


     // if fish caught in area then disable that area sound for good.

    private void Start()
    {
        timeSinceLastGuidance = 0f;
        areaTimer = 0f;
        GetComponent<SphereCollider>().radius = triggerRadius;
        GetComponent<SphereCollider>().isTrigger = true;
        GetComponent<Rigidbody>().useGravity = false;

        playerPos = GameManager.singleton.boat.transform;
        partnerSpeech = GameManager.singleton.partner.GetComponent<PartnerSpeech>();        
    }

    private void FixedUpdate()
    {
        if(timeSinceLastGuidance >= guidanceTimerThreshold)
        {
            playGuidanceSound();
        }
        timeSinceLastGuidance += Time.deltaTime;
        if(previousArea == inArea)
        {
            areaTimer += Time.deltaTime;
            if (areaTimer > areaTimerThreshold)
            {
                detailedAreaSound();
            }
        }
    }
    
    // trigger bakke sound when closest to torsk fishing
    // trugger fugle sound when closest to ål areas
    // trigger Ruse sound when closest to eel trap
    // trigger start sound when ..................
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == start.name && !startHasPlayed)
        {
            startHasPlayed = true;
            partnerSpeech.PartnerSaysSomething(partnerSpeech.Starting);
        }
    }

    void playGuidanceSound()
    {
        //reset the timer
        timeSinceLastGuidance = 0f;
        GameObject closestArea = checkDistance();
        Debug.Log("Closest area: " + closestArea);

        //don't play the first sound again if they are still closest to the same area.
        if (closestArea.tag == inArea)
        {
            return;
        }
        //check which area is the closest
        if(closestArea.tag == "TorskArea")
        {
            partnerSpeech.PartnerSaysSomething(partnerSpeech.Bakke);
        }
        if (closestArea.tag == "EelArea")
        {
            partnerSpeech.PartnerSaysSomething(partnerSpeech.Fugle);
        }
        if (closestArea.tag == "emptyBasket")
        {
            partnerSpeech.PartnerSaysSomething(partnerSpeech.Ruse);
        }
        updateArea(closestArea);
    }

    void updateArea(GameObject area)
    {
        previousArea = inArea;
        inArea = area.tag;
    }

    GameObject checkDistance()
    {
        float shortestDistance = Vector3.Distance(playerPos.position, Torsk.transform.position);
        GameObject closestArea = Torsk.gameObject;
        if(shortestDistance > Vector3.Distance(playerPos.position, eel.transform.position))
        {
            shortestDistance = Vector3.Distance(playerPos.position, eel.transform.position);
            closestArea = eel.gameObject;
        }
        if(shortestDistance > Vector3.Distance(playerPos.position, eelTrap.transform.position))
        {
            shortestDistance = Vector3.Distance(playerPos.position, eelTrap.transform.position);
            closestArea = eelTrap.gameObject;
        }

        return closestArea;
    }

    void detailedAreaSound()
    {
        areaTimer = 0f;
        if(inArea == "TorskArea")
        {
            Debug.Log("Playing detail area sund Torsk");
        }
        if (inArea == "EelArea")
        {
            Debug.Log("Playing detail area sund Eel");
        }
        if (inArea == "emptyBasket")
        {
            Debug.Log("Playing detail area sund EelTrap");
        }
    }

    /*private void OnTriggerEnter(Collider other)
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
    }*/
}
