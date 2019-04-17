using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//[RequireComponent(typeof(LogMaster))]
//main scene needs to have a LogMaster component on this... doesn't need in other scenes
public class GameManager : MonoBehaviour {

    public static GameManager singleton = null;
    //Static instance of GameManager which allows it to be accessed by any other script.                            
    //Current level number, expressed in game as "Day 1".

    [HideInInspector]
    public bool canMove = true;

    public bool useGuidanceSounds = true;
    public bool useMapGuidance = true;
    
    //instances in the scene
    [Header("Object references to be used by other scripts")]
    [HideInInspector]
    public GameObject CameraContainer;
    [HideInInspector]
    public LogMaster logMaster;
    [HideInInspector]
    public GameObject timer;
    [HideInInspector]
    public GameObject boat;
    [HideInInspector]
    public GameObject tribeBoat;
    //[HideInInspector]
    public GameObject partner;
    [HideInInspector]
    public GameObject hook;
    [HideInInspector]
    public GameObject eeliron;
    //public GameObject orca;
    //public GameObject bjørnsholm;
    [HideInInspector]
    public GameObject PelicanEvent;
    [HideInInspector]
    public GameObject spawnPoint;
    public PartnerSpeech guide;
    public GameObject map;
    public GameObject mapOnCam;

    [Header("Prefabs that are probably not used anymore")]
    public GameObject torsk;
	public GameObject eel;
	public GameObject flatFish;
    public GameObject flint;
    public GameObject tradingObject;

    //tools held by guide + the paddle with audio source
    [Header("Tools held by the guide")]
    public GameObject aniTorch;
    public GameObject aniEelIron;
    public GameObject paddle;

    //linear stuff
    bool isCountingTorsk = false;
    int startTorskAmount;
    bool isCountingEel = false;
    int startEelAmount;
    public int currentTorskAmount=0;
    public int currentEelAmount=0;
    public int currentFlatfishAmount=0;

    public bool pointingAtInteractable = false;

    [Header("Linear stuff that is probably not used either")]
    public bool Islinear;
    public List<GameObject> torskArea, eelArea, flatFishArea = new List<GameObject>();
    public GameObject trading;
    public GameObject pillar1,pillar2,pillar3,pillar4,pillar5, currentPillar;

    public GameObject torskTerritory, torskTerritory2, eelTerritory, tribeTerritory;
    public GameObject basket,tribeBasket;
    public GameObject midden;


    List<GameObject> caughtTotal = new List<GameObject>();
    List<GameObject> caughtEel = new List<GameObject>();
    List<GameObject> caughtTorsk =new List<GameObject>();
    List<GameObject> caughtFlatfish =new List<GameObject>();

    //hidden bools for if fish has been caught in their area
    [HideInInspector]
    public bool TorskCaught = false;
    [HideInInspector]
    public bool eelCaught = false;
    [HideInInspector]
    public bool eelTrapEmptied = false;
    [HideInInspector]
    public bool flatFishCaught = false;

    // Audio Sources
    [HideInInspector]
    public AudioSource _audio;
    //for change to end scene:
    bool hasFlint = false;

    //Awake is always called before any Start functions
    void Awake()
    {
        //resetFishInBasket();
        //Check if instance already exists
        if (singleton == null)
                
            //if not, set instance to this
            singleton = this;
            
        //If instance already exists and it's not this:
        else if (singleton != this)
                
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);    
            
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        CameraContainer = GameObject.FindGameObjectWithTag("Guidance");
        if(GetComponent<LogMaster>() != null)
        {
            logMaster = GetComponent<LogMaster>();
        }
        else
        {
            Debug.LogError("GameManager object have no logMaster component");
        }

        timer = GameObject.FindGameObjectWithTag("timer");
        //Debug.Log(boat.gameObject.name); 
        boat = GameObject.FindGameObjectWithTag("boat");
        //Debug.Log(boat.gameObject.name);      
        //bjørnsholm = GameObject.FindGameObjectWithTag("bjørnholm");
        //Debug.Log(boat.gameObject.name);            
        tribeBoat = GameObject.FindGameObjectWithTag("tribeBoat");
        //Debug.Log(tribeBoat.gameObject.name);
        partner = GameObject.FindGameObjectWithTag("partner");
        //Debug.Log(partner.gameObject.name);
        hook = GameObject.FindGameObjectWithTag("hook");
        //Debug.Log(hook.gameObject.name);
        eeliron = GameObject.FindGameObjectWithTag("eeliron");
        //Debug.Log(eeliron.gameObject.name);
        paddle = GameObject.FindGameObjectWithTag("paddle");
        //orca = GameObject.FindGameObjectWithTag("orca");
        //Debug.Log(orca.gameObject.name);
        PelicanEvent = GameObject.FindGameObjectWithTag("flyingPelican");
        //Debug.Log(PelicanEvent.gameObject.name);
        try{
        PelicanEvent.SetActive(false);
        }catch{}

            
        currentPillar = boat;

        //what should be turned of initially for the linear condition
           
        //what should be turned of initially for both condition
            try{
            midden.GetComponent<Collider>().enabled = false;
            pillar1.SetActive(false);
            pillar2.SetActive(false);
            pillar3.SetActive(false);
            pillar4.SetActive(false);
            pillar5.SetActive(false);

            } catch{}
        _audio = GetComponent<AudioSource>();

        Physics.IgnoreLayerCollision(0, 11);
                
    }
    private void Start()
    {
        
        if (!useMapGuidance)
        {
            map.SetActive(false);
            mapOnCam.SetActive(false);
        }
        if(!useGuidanceSounds)
        {
            StartCoroutine(makeSureStartSoundPlays(guide.StartingSoundGoFishing));
        }
    }

    public void AddEel(GameObject eel)
        {
            caughtTotal.Add(eel);
            currentEelAmount++;
        
        }
        public void AddTorsk(GameObject torsk)
        {
            caughtTotal.Add(torsk);
            currentTorskAmount++;
        }
        public void AddFlatFish(GameObject flat)
        {
            caughtTotal.Add(flat);
            currentFlatfishAmount++;
        }

        public int GetFishCount()
        {
            return caughtTotal.Count;
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("i am in end scene");
        Debug.Log("OnSceneLoaded: " + scene.name);
        //audio.Play();
        Debug.Log("after loading i have these fish: Torsk = " + currentTorskAmount + " Eel = " + currentEelAmount + " Flatfish = " + currentFlatfishAmount);
        for (int i = 1; i <= currentFlatfishAmount; i++)
        {
        Debug.Log("I have a flatfish");
        Transform[] trans = GameObject.FindGameObjectWithTag("basket").GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trans)
            {
                if (t.gameObject.name == "flatfish_Caught_0"+i) 
                {
                    t.gameObject.SetActive(true);
                }
            }                
        }
        for (int i = 1; i <= currentEelAmount; i++)
        {
            Debug.Log("I have an eel");
            Transform[] trans = GameObject.FindGameObjectWithTag("basket").GetComponentsInChildren<Transform>(true);
            foreach (Transform t in trans) {
                if (t.gameObject.name == "eel_Caught_0"+i) 
                {
                    t.gameObject.SetActive(true);
                }
            }                
        }
        for (int i = 1; i <= currentTorskAmount; i++)
        {
            Debug.Log("I have a torsk");
            Transform[] trans = GameObject.FindGameObjectWithTag("basket").GetComponentsInChildren<Transform>(true);
            foreach (Transform t in trans) {
                if (t.gameObject.name == "torsk_Caught_0"+i) 
                {
                    t.gameObject.SetActive(true);
                }
            }                
        }
    }
    /*private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }*/

    public void PrepareForEndScene(AudioClip clip, bool hasFlint)
    {
            
        _audio.clip = clip;
        this.hasFlint = hasFlint;
    }

    public int getTotalFishCaught()
    {
        int totalFish = 0;

        foreach (GameObject fish in caughtTotal)
        {
            totalFish++;
        }
        return totalFish;
    }

    IEnumerator makeSureStartSoundPlays(AudioClip clip)
    {
        yield return new WaitForSeconds(clip.length);
        CameraContainer.GetComponent<GuidanceSounds>().enabled = false;
    }

    public void resetFishInBasket()
    {
        currentEelAmount = 0;
        currentFlatfishAmount = 0;
        currentTorskAmount = 0;
        foreach (GameObject fish in caughtTotal)
        {
            caughtTotal.Remove(fish);
        }
    }
}
