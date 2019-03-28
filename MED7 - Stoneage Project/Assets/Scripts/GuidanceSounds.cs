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
    public Collider flatFish;
    public Collider start;

    public float timeSinceLastGuidance;
    public float guidanceTimerThreshold = 40f;
    public float areaTimer;
    public float areaTimerThreshold = 20f;

    public static string lastGuidanceSound;

    string inArea = "none";
    string previousArea = "different none";

    /*bool torskHasPlayed = false;
    bool eelHasPlayed = false;
    bool eeltrapHasPlayed = false;*/
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
        //Debug.Log("Previous area: " + previousArea + " = Current area: " + inArea);
        if(previousArea == inArea)
        {
            areaTimer += Time.deltaTime;
            if (areaTimer > areaTimerThreshold)
            {
                detailedAreaSound();
            }
        }
        timeSinceLastGuidance += Time.deltaTime;
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
            partnerSpeech.PartnerSaysSomething(partnerSpeech.StartingSoundGoFishing);
        }
    }

    public void playGuidanceSound()
    {
        if(checkDistance() == null)
        {
            Debug.Log("Closest distance was null, either all areas have been tried or something went wrong");
            return;
        }
        //reset the timer
        timeSinceLastGuidance = 0f;
        GameObject closestArea = checkDistance();

        bool lvl1GuidanceSound = true;
        //don't play the first sound again if they are still closest to the same area.
        if (closestArea.tag == inArea)
        {
            Debug.Log("Closest area is unchanged");
            lvl1GuidanceSound = false;
        }
        Debug.Log("Closest area: " + closestArea);
        //check which area is the closest
        if(closestArea.tag == "TorskArea" && lvl1GuidanceSound && !GameManager.singleton.TorskCaught)
        {
            partnerSpeech.PartnerSaysSomething(partnerSpeech.ClosestToTorsk);
            setLastGuidanceSound(partnerSpeech.ClosestToTorsk);
        }
        if (closestArea.tag == "EelArea" && lvl1GuidanceSound && !GameManager.singleton.eelCaught)
        {
            partnerSpeech.PartnerSaysSomething(partnerSpeech.ClosestToEel);
            setLastGuidanceSound(partnerSpeech.ClosestToEel);
        }
        if (closestArea.tag == "emptyBasket" && lvl1GuidanceSound && !GameManager.singleton.eelTrapEmptied)
        {
            partnerSpeech.PartnerSaysSomething(partnerSpeech.ClosestToEeltrap);
            setLastGuidanceSound(partnerSpeech.ClosestToEeltrap);
        }
        if (closestArea.tag == "FlatfishArea" && lvl1GuidanceSound && !GameManager.singleton.flatFishCaught)
        {
            //Debug.Log("the flatfish area is over there... Sound is suppose to play");
            partnerSpeech.PartnerSaysSomething(partnerSpeech.ClosestToFlatfish);
            setLastGuidanceSound(partnerSpeech.ClosestToFlatfish);
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
        GameObject closestArea = null;
        float shortestDistance = 9999999;
        if(shortestDistance > Vector3.Distance(playerPos.position, Torsk.transform.position) && !GameManager.singleton.TorskCaught)
        {
            shortestDistance = Vector3.Distance(playerPos.position, Torsk.transform.position);
            closestArea = Torsk.gameObject;
        }
        if(shortestDistance > Vector3.Distance(playerPos.position, eel.transform.position) && !GameManager.singleton.eelCaught)
        {
            shortestDistance = Vector3.Distance(playerPos.position, eel.transform.position);
            closestArea = eel.gameObject;
        }
        if(shortestDistance > Vector3.Distance(playerPos.position, eelTrap.transform.position) && !GameManager.singleton.eelTrapEmptied)
        {
            shortestDistance = Vector3.Distance(playerPos.position, eelTrap.transform.position);
            closestArea = eelTrap.gameObject;
        }
        if (shortestDistance > Vector3.Distance(playerPos.position, flatFish.transform.position) && !GameManager.singleton.flatFishCaught)
        {
            shortestDistance = Vector3.Distance(playerPos.position, flatFish.transform.position);
            closestArea = flatFish.gameObject;
        }

        return closestArea;
    }

    void detailedAreaSound()
    {
        areaTimer = 0f;
        if(inArea == "TorskArea" && !GameManager.singleton.TorskCaught)
        {
            //Debug.Log("Playing detail area sund Torsk");
            partnerSpeech.PartnerSaysSomething(partnerSpeech.DetailClosestToTorsk);
            setLastGuidanceSound(partnerSpeech.DetailClosestToTorsk);
        }
        if (inArea == "EelArea" && !GameManager.singleton.eelCaught)
        {
            //Debug.Log("Playing detail area sund Eel");
            partnerSpeech.PartnerSaysSomething(partnerSpeech.DetailClosestToEel);
            setLastGuidanceSound(partnerSpeech.DetailClosestToEel);
        }
        if (inArea == "emptyBasket" && !GameManager.singleton.eelTrapEmptied)
        {
            //Debug.Log("Playing detail area sund EelTrap");
            partnerSpeech.PartnerSaysSomething(partnerSpeech.DetailClosestToEeltrap);
            setLastGuidanceSound(partnerSpeech.DetailClosestToEeltrap);
        }
        if (inArea == "FlatfishArea" && !GameManager.singleton.flatFishCaught)
        {
            //Debug.Log("Playing detail area sund Flatfish");
            partnerSpeech.PartnerSaysSomething(partnerSpeech.DetailClosestToFlatfish);
            setLastGuidanceSound(partnerSpeech.DetailClosestToFlatfish);
        }
    }

    void setLastGuidanceSound(AudioClip clip)
    {
        lastGuidanceSound = clip.name;
    }
}
