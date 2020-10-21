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

    [Header("Thresholds")]
    public static float timeSinceLastGuidance;
    [SerializeField] private float guidanceTimerThreshold = 35f;

    public static float areaTimer;
    [SerializeField] private float areaTimerThreshold = 20f;

    [SerializeField] private float farDistanceGuideExclude = 200f;
    private float maxAnglethreshold = 180f; //from vector2.Angle calls

    [Header("Weights")]
    [SerializeField] private float weightForAngle = 1f;
    [SerializeField] private float weightForDistance = 1f;

    [Header("other assignables")]
    //[SerializeField] private BoxCollider boatForwardTrigger;

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
    Transform player;
    private int numberToExclude;


    private static int detailedIndex = 0;

    private Camera _cam;
    public static bool isSailingTowardsFishArea = false;

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

        player = GameManager.singleton.boat.transform;
        partnerSpeech = GameManager.singleton.partner.GetComponent<PartnerSpeech>();

        fishAreaDatas = new List<AreaDataContainer>();
        AddAreaContainerDefualtData(fishAreaDatas);
        _cam = Camera.main;
        //SetupBoatForwardTrigger();
    }

    private void AddAreaContainerDefualtData(List<AreaDataContainer> mlist)
    {
        AreaDataContainer Mtorsk = new AreaDataContainer("Torsk area", Torsk.gameObject, farDistanceGuideExclude, maxAnglethreshold,weightForDistance,weightForAngle);
        AreaDataContainer Mflatfish = new AreaDataContainer("Flatfish area", flatFish.gameObject, farDistanceGuideExclude, maxAnglethreshold, weightForDistance, weightForAngle);
        AreaDataContainer Meel = new AreaDataContainer("Eel area", eel.gameObject, farDistanceGuideExclude, maxAnglethreshold, weightForDistance, weightForAngle);
        AreaDataContainer MeelTrap = new AreaDataContainer("EelTrap Region (1)", eelTrap.gameObject,farDistanceGuideExclude,maxAnglethreshold, weightForDistance, weightForAngle);

        mlist.Add(Mtorsk);
        mlist.Add(Mflatfish);
        mlist.Add(Meel);
        mlist.Add(MeelTrap);
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
    

    public void PlayGuidanceSound()
    {
        //reset the timer
        timeSinceLastGuidance = 0f;

        ChooseBestAreaToGuideTowards();
        if(_currentBestAreaToGuideTowards == null)
        {
            return;
        }

        bool lvl1GuidanceSound = true;
        //don't play the first sound again if they are still closest to the same area.
        if (_currentBestAreaToGuideTowards.tag == SelectedArea)
        {
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
        if (_currentBestAreaToGuideTowards.tag == "EelTrapArea" && lvl1GuidanceSound && !GameManager.singleton.eelTrapEmptied)
        {
            partnerSpeech.PartnerSaysSomething(partnerSpeech.ClosestToEeltrap);
            SetLastGuidanceSound(partnerSpeech.ClosestToEeltrap);
        }
        if (_currentBestAreaToGuideTowards.tag == "FlatfishArea" && lvl1GuidanceSound && !GameManager.singleton.flatFishCaught)
        {
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
        UpdateFishAreaData();

        _currentBestAreaToGuideTowards = GetAreaWithLowestScore();
    }

    private GameObject CheckUnseenAreas()
    {
        float lowestScore = 99999999;
        GameObject lowestScoreArea = null;
        foreach (AreaDataContainer fishArea in fishAreaDatas)
        {
            //check only unseen areas
            if (fishArea.canAreaBeSeen)
                continue;
            //check if area has already been fished in
            if (fishArea.name.Equals("Torsk area") && GameManager.singleton.TorskCaught )
                continue;
            if (fishArea.name.Equals("Flatfish area") && GameManager.singleton.flatFishCaught)
                continue;
            if (fishArea.name.Equals("Eel area") && GameManager.singleton.eelCaught)
                continue;
            if (fishArea.name.Equals("EelTrap Region (1)") && GameManager.singleton.eelTrapEmptied)
                continue;

            if (lowestScore > fishArea.guidanceScore && !fishArea.canAreaBeSeen)
            {
                lowestScore = fishArea.guidanceScore;
                lowestScoreArea = fishArea.gObject;
            }
        }

        return lowestScoreArea;
    }

    private void UpdateFishAreaData()
    {
        foreach (AreaDataContainer fishArea in fishAreaDatas)
        {
            Vector2 horizontalFishAreaPos = new Vector2(fishArea.position.x, fishArea.position.z);
            fishArea.distanceFromPlayer = GetDistanceFromPlayer(horizontalFishAreaPos);

            fishArea.horizontalAngleFromLookDirection = GetHorizontalAnglefromPlayer(fishArea.position);
            

            Vector3 dir = fishArea.position - transform.position;
            //if true then we hit something
            if(Physics.Raycast(transform.position, dir.normalized, dir.magnitude, 
                1 << LayerMask.NameToLayer("GuidanceRayBlocker")))
            {
                fishArea.canAreaBeSeen = false;
            }
            else
            {
                fishArea.canAreaBeSeen = true;
            }
        }
    }

    private GameObject GetAreaWithLowestScore()
    {
        float lowestScore = 99999999;
        GameObject lowestScoreArea = null;
        foreach (AreaDataContainer fishArea in fishAreaDatas)
        {
            if (fishArea.name.Equals("Torsk area") && GameManager.singleton.TorskCaught)
                continue;
            if (fishArea.name.Equals("Flatfish area") && GameManager.singleton.flatFishCaught)
                continue;
            if (fishArea.name.Equals("Eel area") && GameManager.singleton.eelCaught)
                continue;
            if (fishArea.name.Equals("EelTrap Region (1)") && GameManager.singleton.eelTrapEmptied)
                continue;

            if (lowestScore > fishArea.guidanceScore && fishArea.canAreaBeSeen)
            {
                lowestScore = fishArea.guidanceScore;
                lowestScoreArea = fishArea.gObject;
            }
        }

        if(lowestScoreArea == null)
        {
            return CheckUnseenAreas();
        }
        return lowestScoreArea;
    }

    /// <summary>
    /// Returns horizontal distance from the player to the target.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private float GetDistanceFromPlayer(Vector2 target)
    {
        Vector2 playerPosition = new Vector2(player.position.x, player.position.z);
        float distance = Vector2.Distance(playerPosition, target);
        return distance;
    }

    /// <summary>
    /// Returns the horizontal angle between the player and the target.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private float GetHorizontalAnglefromPlayer(Vector3 target)
    {
        Vector3 playerToTarget = target - _cam.transform.position;
        playerToTarget.y = 0;
        Vector3 viewDir = GetHorizontalViewDirection(_cam);
        viewDir.y = 0;
        float angle = Vector3.Angle(playerToTarget, viewDir);
        return angle;
    }
    

    private GameObject GetclosestArea()
    {
        GameObject closestArea = null;
        float shortestDistance = 9999999f;

        foreach (AreaDataContainer fishArea in fishAreaDatas)
        {
            if (fishArea.name.Equals("Torsk area") && GameManager.singleton.TorskCaught)
                continue;
            if (fishArea.name.Equals("Flatfish area") && GameManager.singleton.flatFishCaught)
                continue;
            if (fishArea.name.Equals("Eel area") && GameManager.singleton.eelCaught)
                continue;
            if (fishArea.name.Equals("EelTrap Region (1)") && GameManager.singleton.eelTrapEmptied)
                continue;
            if(shortestDistance > fishArea.distanceFromPlayer)
            {
                shortestDistance = fishArea.distanceFromPlayer;
                closestArea = fishArea.gObject;
            }
        }
        return closestArea;
    }

    void DetailedAreaSound()
    {
        //reset the timer
        areaTimer = 0f;

        ChooseBestAreaToGuideTowards();
        if (_currentBestAreaToGuideTowards == null)
        {
            return;
        }

        //reset which guidance timer to use
        if (_currentBestAreaToGuideTowards.tag != SelectedArea)
        {
            //change which guidance timer to use
            detailedGuidance = false;
            standardGuidance = true;
            resetDetailedIndex();
            return;
        }
        if(SelectedArea == "TorskArea" && !GameManager.singleton.TorskCaught)
        {
            partnerSpeech.PartnerSaysSomething(DetailedArrayInOrderSound(partnerSpeech.DetailClosestToTorsk));
        }
        if (SelectedArea == "EelArea" && !GameManager.singleton.eelCaught)
        {
            partnerSpeech.PartnerSaysSomething(DetailedArrayInOrderSound(partnerSpeech.DetailClosestToEel));
        }
        if (SelectedArea == "EelTrapArea" && !GameManager.singleton.eelTrapEmptied)
        {
            partnerSpeech.PartnerSaysSomething(DetailedArrayInOrderSound(partnerSpeech.DetailClosestToEeltrap));
        }
        if (SelectedArea == "FlatfishArea" && !GameManager.singleton.flatFishCaught)
        {
            partnerSpeech.PartnerSaysSomething(DetailedArrayInOrderSound(partnerSpeech.DetailClosestToFlatfish));
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
    /// <param name="seconds"></param>
    public void AddTimeToGuidanceActiveTimer(float seconds)
    {
        if (standardGuidance)
        {
            timeSinceLastGuidance -= seconds;
        }
        if (detailedGuidance)
        {
            areaTimer -= seconds;
        }
    }

    public Vector3 GetHorizontalViewDirection(Camera cam)
    {
        Vector3 viewDir = cam.transform.forward;
        viewDir.y = cam.transform.position.y;
        return viewDir * 300;
    }

    AudioClip RandomDetailedSound(AudioClip[] clipArray)
    {
        List<int> numbersToChooseFrom = new List<int> { };
        for (int i = 0; i < clipArray.Length; i++)
        {
            numbersToChooseFrom.Add(i);
        }

        numbersToChooseFrom.Remove(numberToExclude);

        int randomIndex = Random.Range(0, numbersToChooseFrom.Count);
        int randomNumber = numbersToChooseFrom[randomIndex];
        SetLastGuidanceSound(clipArray[randomNumber]);
        //exclude number here for next time
        numberToExclude = randomNumber;
        return clipArray[randomNumber];
    }

    AudioClip DetailedArrayInOrderSound(AudioClip[] clipArray)
    {
        //make sure does not go out of range
        if(detailedIndex > clipArray.Length - 1)
        {
            detailedIndex = 0; //makes sure the detailed guidance loops
        }
        AudioClip clipToPlay = clipArray[detailedIndex];
        //setLastGuidanceSound(clipArray[detailedIndex]);
        detailedIndex++;
        return clipToPlay;
    }

    void resetDetailedIndex()
    {
        detailedIndex = 0;
    }

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

    

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            //draw far distance of when distance score starts getting discarded
            transform.DrawGizmoDisk(farDistanceGuideExclude, Color.red);
            

            //draw line in view direction
            Gizmos.color = Color.green;
            Gizmos.DrawRay(_cam.transform.position, GetHorizontalViewDirection(_cam));

            //draw line to all fish areas
            Gizmos.color = Color.gray;
            foreach (AreaDataContainer fishArea in fishAreaDatas)
            {
                Vector3 tempFishPos = fishArea.position;
                tempFishPos.y = _cam.transform.position.y;
                Gizmos.DrawLine(_cam.transform.position, tempFishPos);
            }
        }
    }
}
