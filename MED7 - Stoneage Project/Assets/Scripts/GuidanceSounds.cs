using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class GuidanceSounds : MonoBehaviour {
    //public PartnerSpeech voiceLines;
    [HideInInspector]
    //public float triggerRadius = 150; //this is not used anymore except to activate the start sound

    [Header("Trigger areas")]
    public Collider Torsk;
    public Collider eel;
    public Collider eelTrap;
    public Collider flatFish;
    public Collider start;

    public static float timeSinceLastGuidance;
    public float guidanceTimerThreshold = 35f;
    public static float areaTimer;
    public float areaTimerThreshold = 20f;

    public static string lastGuidanceSound;

    public static bool standardGuidance = true;
    public static bool detailedGuidance = false;

    string inArea = "none";
    string previousArea = "different none";
    GameObject closestArea;

    public static bool startHasPlayed = false;

    PartnerSpeech partnerSpeech;
    Transform playerPos;
    private int numberToExclude;

    private static int detailedIndex = 0;

     // if fish caught in area then disable that area sound for good.

    private void Start()
    {
        standardGuidance = true;
        detailedGuidance = false;
        detailedIndex = 0;
        startHasPlayed = false;
        timeSinceLastGuidance = 0f;
        areaTimer = 0f;
        //GetComponent<SphereCollider>().radius = triggerRadius;
        GetComponent<SphereCollider>().isTrigger = true;
        GetComponent<Rigidbody>().useGravity = false;

        playerPos = GameManager.singleton.boat.transform;
        partnerSpeech = GameManager.singleton.partner.GetComponent<PartnerSpeech>();        
    }

    private void FixedUpdate()
    {
        updateGuidanceTimers();
    }
    
    void updateGuidanceTimers()
    {
        if (timeSinceLastGuidance >= guidanceTimerThreshold)
        {
            playGuidanceSound();
        }
        //Debug.Log("Previous area: " + previousArea + " = Current area: " + inArea);
        if (detailedGuidance)
        {
            if (areaTimer > areaTimerThreshold)
            {
                detailedAreaSound();
            }
            areaTimer += Time.deltaTime;
        }
        if (standardGuidance)
        {
            timeSinceLastGuidance += Time.deltaTime;
        }
    }
    
    //plays the start sound once
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Regions"))
            return;
        if (other.name == start.name && !startHasPlayed)
        {
            startHasPlayed = true;
            partnerSpeech.PartnerSaysSomething(partnerSpeech.StartingSoundGoFishing);
        }
    }

    public void playGuidanceSound()
    {
        if(checkDistance() == null)
        {
            //Debug.Log("Closest distance was null, either all areas have been tried or something went wrong");
            return;
        }
        //reset the timer
        timeSinceLastGuidance = 0f;
        closestArea = checkDistance();

        bool lvl1GuidanceSound = true;
        //don't play the first sound again if they are still closest to the same area.
        if (closestArea.tag == inArea)
        {
            //Debug.Log("Closest area is unchanged");
            lvl1GuidanceSound = false;
            //change which guidance timer to use
            detailedGuidance = true;
            standardGuidance = false;
            detailedAreaSound();
        }
        //Debug.Log("Closest area: " + closestArea);
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
        if (checkDistance() == null)
        {
            Debug.Log("Closest distance was null, either all areas have been tried or something went wrong");
            return;
        }
        //reset the timer
        areaTimer = 0f;
        closestArea = checkDistance();

        //reset which guidance timer to use
        if (closestArea.tag != inArea)
        {
            Debug.Log("Closest area is new");
            //change which guidance timer to use
            detailedGuidance = false;
            standardGuidance = true;
            resetDetailedIndex();
            return;
        }
        if(inArea == "TorskArea" && !GameManager.singleton.TorskCaught)
        {
            //Debug.Log("Playing detail area sund Torsk");
            //partnerSpeech.PartnerSaysSomething(randomDetailedSound(partnerSpeech.DetailClosestToTorsk));
            partnerSpeech.PartnerSaysSomething(detailedArrayInOrderSound(partnerSpeech.DetailClosestToTorsk));
            setLastGuidanceSound(detailedArrayInOrderSound(partnerSpeech.DetailClosestToTorsk));
        }
        if (inArea == "EelArea" && !GameManager.singleton.eelCaught)
        {
            //Debug.Log("Playing detail area sund Eel");
            //partnerSpeech.PartnerSaysSomething(randomDetailedSound(partnerSpeech.DetailClosestToEel));
            partnerSpeech.PartnerSaysSomething(detailedArrayInOrderSound(partnerSpeech.DetailClosestToEel));
            setLastGuidanceSound(detailedArrayInOrderSound(partnerSpeech.DetailClosestToEel));
        }
        if (inArea == "emptyBasket" && !GameManager.singleton.eelTrapEmptied)
        {
            //Debug.Log("Playing detail area sund EelTrap");
            //partnerSpeech.PartnerSaysSomething(randomDetailedSound(partnerSpeech.DetailClosestToEeltrap));
            partnerSpeech.PartnerSaysSomething(detailedArrayInOrderSound(partnerSpeech.DetailClosestToEeltrap));
            setLastGuidanceSound(detailedArrayInOrderSound(partnerSpeech.DetailClosestToEeltrap));
        }
        if (inArea == "FlatfishArea" && !GameManager.singleton.flatFishCaught)
        {
            //Debug.Log("Playing detail area sund Flatfish");
            //partnerSpeech.PartnerSaysSomething(randomDetailedSound(partnerSpeech.DetailClosestToFlatfish));
            partnerSpeech.PartnerSaysSomething(detailedArrayInOrderSound(partnerSpeech.DetailClosestToFlatfish));
            setLastGuidanceSound(detailedArrayInOrderSound(partnerSpeech.DetailClosestToFlatfish));
        }
        updateArea(closestArea);
    }

    void setLastGuidanceSound(AudioClip clip)
    {
        lastGuidanceSound = clip.name;
    }

    public void resetGuidanceTimers()
    {
        //reset is called in PartnerAnimator when a fish is caught
        standardGuidance = true;
        resetDetailedIndex();
        detailedGuidance = false;
        timeSinceLastGuidance = 0f;
        areaTimer = 0f;
    }

    public void addVoiceTimeToActiveTimer(float clipTime)
    {
        if (standardGuidance)
        {
            //Debug.Log("voiceclip is altering Standard guidance timer");
            timeSinceLastGuidance -= clipTime;
        }
        if (detailedGuidance)
        {
            //Debug.Log("voiceclip is altering Detailed guidance timer");
            areaTimer -= clipTime;
        }
    }

    AudioClip randomDetailedSound(AudioClip[] clipArray)
    {
        List<int> numbersToChooseFrom = new List<int> { };
        for (int i = 0; i < clipArray.Length; i++)
        {
            numbersToChooseFrom.Add(i);
        }

        //Debug.Log("number not to choose detailed = " + numberToExclude);
        numbersToChooseFrom.Remove(numberToExclude);
        /*foreach (int number in numbersToChooseFrom)
        {
            Debug.Log("numbers to choose from contains: " + number);
        }*/
        int randomIndex = Random.Range(0, numbersToChooseFrom.Count);
        int randomNumber = numbersToChooseFrom[randomIndex];
        //Debug.Log("index that have been chosen for the audioclip array: " + randomNumber);
        setLastGuidanceSound(clipArray[randomNumber]);
        //exclude number here for next time
        numberToExclude = randomNumber;
        return clipArray[randomNumber];
    }

    AudioClip detailedArrayInOrderSound(AudioClip[] clipArray)
    {
        //make sure does not go out of range
        if(detailedIndex >= clipArray.Length - 1)
        {
            detailedIndex = clipArray.Length - 1;
        }
        AudioClip clipToPlay = clipArray[detailedIndex];
        //setLastGuidanceSound(clipArray[detailedIndex]);
        Debug.Log("detailed guide array index is at: " + detailedIndex);
        detailedIndex++;
        return clipToPlay;
    }

    void resetDetailedIndex()
    {
        detailedIndex = 0;
        Debug.Log("Detailed array index has been reset");
    } 
}
