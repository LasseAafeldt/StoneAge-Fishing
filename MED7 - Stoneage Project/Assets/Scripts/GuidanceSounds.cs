using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class GuidanceSounds : MonoBehaviour {

    [Header("Trigger areas")]
    [SerializeField] private Collider Torsk;
    [SerializeField] private Collider eel;
    [SerializeField] private Collider eelTrap;
    [SerializeField] private Collider flatFish;
    [SerializeField] private Collider start;

    public static float timeSinceLastGuidance;
    [SerializeField] private float guidanceTimerThreshold = 35f;

    public static float areaTimer;
    [SerializeField] private float areaTimerThreshold = 20f;

    [SerializeField] private List<AreaDataContainer> fishAreaDatas;

    public static string lastGuidanceSound;

    public static bool standardGuidance = true;
    public static bool detailedGuidance = false;

    string SelectedArea = "none";
    string previousArea = "different none";
    //GameObject closestArea;
    private GameObject _currentBestAreaToGuideTowards;

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

        fishAreaDatas = new List<AreaDataContainer>();
    }

    private void AddAreaContainerDefualtData(List<AreaDataContainer> mlist)
    {
        AreaDataContainer torsk = new AreaDataContainer();
        torsk.name = "Torsk area";
        torsk.distanceFromPlayer = 999999f;
        torsk.horizontalAngleFromLookDirection = 0f;
    }

    private void FixedUpdate()
    {
        UpdateGuidanceTimers();
    }
    
    void UpdateGuidanceTimers()
    {
        if (timeSinceLastGuidance >= guidanceTimerThreshold)
        {
            PlayGuidanceSound();
        }
        if (detailedGuidance)
        {
            if (areaTimer > areaTimerThreshold)
            {
                DetailedAreaSound();
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

    public void PlayGuidanceSound()
    {
        ChooseBestAreaToGuideTowards();
        if(_currentBestAreaToGuideTowards == null)
        {
            //Debug.Log("Closest distance was null, either all areas have been tried or something went wrong");
            return;
        }
        //reset the timer
        timeSinceLastGuidance = 0f;

        bool lvl1GuidanceSound = true;
        //don't play the first sound again if they are still closest to the same area.
        if (_currentBestAreaToGuideTowards.tag == SelectedArea)
        {
            //Debug.Log("Closest area is unchanged");
            lvl1GuidanceSound = false;
            //change which guidance timer to use
            detailedGuidance = true;
            standardGuidance = false;
            DetailedAreaSound();
        }

        //check which area is the closest
        if(_currentBestAreaToGuideTowards.tag == "TorskArea" && lvl1GuidanceSound && !GameManager.singleton.TorskCaught)
        {
            partnerSpeech.PartnerSaysSomething(partnerSpeech.ClosestToTorsk);
            SetLastGuidanceSound(partnerSpeech.ClosestToTorsk);
        }
        if (_currentBestAreaToGuideTowards.tag == "EelArea" && lvl1GuidanceSound && !GameManager.singleton.eelCaught)
        {
            partnerSpeech.PartnerSaysSomething(partnerSpeech.ClosestToEel);
            SetLastGuidanceSound(partnerSpeech.ClosestToEel);
        }
        if (_currentBestAreaToGuideTowards.tag == "emptyBasket" && lvl1GuidanceSound && !GameManager.singleton.eelTrapEmptied)
        {
            partnerSpeech.PartnerSaysSomething(partnerSpeech.ClosestToEeltrap);
            SetLastGuidanceSound(partnerSpeech.ClosestToEeltrap);
        }
        if (_currentBestAreaToGuideTowards.tag == "FlatfishArea" && lvl1GuidanceSound && !GameManager.singleton.flatFishCaught)
        {
            //Debug.Log("the flatfish area is over there... Sound is suppose to play");
            partnerSpeech.PartnerSaysSomething(partnerSpeech.ClosestToFlatfish);
            SetLastGuidanceSound(partnerSpeech.ClosestToFlatfish);
        }

        UpdateArea(_currentBestAreaToGuideTowards);
    }

    void UpdateArea(GameObject area)
    {
        previousArea = SelectedArea;
        SelectedArea = area.tag;
    }

    //chooses an area to give guidance towards
    private void ChooseBestAreaToGuideTowards()
    {

        GameObject closestArea = GetclosestArea();

        //_currentBestAreaToGuideTowards = ;
    }


    private GameObject GetclosestArea()
    {
        GameObject closestArea = null;
        float shortestDistance = 9999999;
        if (shortestDistance > Vector3.Distance(playerPos.position, Torsk.transform.position) && !GameManager.singleton.TorskCaught)
        {
            shortestDistance = Vector3.Distance(playerPos.position, Torsk.transform.position);
            closestArea = Torsk.gameObject;
        }
        if (shortestDistance > Vector3.Distance(playerPos.position, eel.transform.position) && !GameManager.singleton.eelCaught)
        {
            shortestDistance = Vector3.Distance(playerPos.position, eel.transform.position);
            closestArea = eel.gameObject;
        }
        if (shortestDistance > Vector3.Distance(playerPos.position, eelTrap.transform.position) && !GameManager.singleton.eelTrapEmptied)
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

    void DetailedAreaSound()
    {
        ChooseBestAreaToGuideTowards();
        if (_currentBestAreaToGuideTowards == null)
        {
            Debug.Log("Closest distance was null, either all areas have been tried or something went wrong");
            return;
        }
        //reset the timer
        areaTimer = 0f;

        //reset which guidance timer to use
        if (_currentBestAreaToGuideTowards.tag != SelectedArea)
        {
            Debug.Log("Closest area is new");
            //change which guidance timer to use
            detailedGuidance = false;
            standardGuidance = true;
            resetDetailedIndex();
            return;
        }
        if(SelectedArea == "TorskArea" && !GameManager.singleton.TorskCaught)
        {
            partnerSpeech.PartnerSaysSomething(DetailedArrayInOrderSound(partnerSpeech.DetailClosestToTorsk));
            SetLastGuidanceSound(DetailedArrayInOrderSound(partnerSpeech.DetailClosestToTorsk));
        }
        if (SelectedArea == "EelArea" && !GameManager.singleton.eelCaught)
        {
            partnerSpeech.PartnerSaysSomething(DetailedArrayInOrderSound(partnerSpeech.DetailClosestToEel));
            SetLastGuidanceSound(DetailedArrayInOrderSound(partnerSpeech.DetailClosestToEel));
        }
        if (SelectedArea == "emptyBasket" && !GameManager.singleton.eelTrapEmptied)
        {
            partnerSpeech.PartnerSaysSomething(DetailedArrayInOrderSound(partnerSpeech.DetailClosestToEeltrap));
            SetLastGuidanceSound(DetailedArrayInOrderSound(partnerSpeech.DetailClosestToEeltrap));
        }
        if (SelectedArea == "FlatfishArea" && !GameManager.singleton.flatFishCaught)
        {
            partnerSpeech.PartnerSaysSomething(DetailedArrayInOrderSound(partnerSpeech.DetailClosestToFlatfish));
            SetLastGuidanceSound(DetailedArrayInOrderSound(partnerSpeech.DetailClosestToFlatfish));
        }
        UpdateArea(_currentBestAreaToGuideTowards);
    }

    /// <summary>
    /// Sets the last played guidance sound for logging purposes
    /// </summary>
    /// <param name="clip"></param>
    void SetLastGuidanceSound(AudioClip clip)
    {
        lastGuidanceSound = clip.name;
    }

    public void ResetGuidanceTimers()
    {
        //reset is called in PartnerAnimator when a fish is caught
        standardGuidance = true;
        resetDetailedIndex();
        detailedGuidance = false;
        timeSinceLastGuidance = 0f;
        areaTimer = 0f;
    }

    /// <summary>
    /// adds time from a played voiceline to the guidance interval timer(s)
    /// </summary>
    /// <param name="clipTime"></param>
    public void AddVoiceTimeToActiveTimer(float clipTime)
    {
        if (standardGuidance)
        {
            timeSinceLastGuidance -= clipTime;
        }
        if (detailedGuidance)
        {
            areaTimer -= clipTime;
        }
    }

    AudioClip RandomDetailedSound(AudioClip[] clipArray)
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
        SetLastGuidanceSound(clipArray[randomNumber]);
        //exclude number here for next time
        numberToExclude = randomNumber;
        return clipArray[randomNumber];
    }

    AudioClip DetailedArrayInOrderSound(AudioClip[] clipArray)
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
