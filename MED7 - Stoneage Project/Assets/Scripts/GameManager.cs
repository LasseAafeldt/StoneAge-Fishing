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

    public bool canMove = true;
    //instances in the scene

    public GameObject CameraContainer;
    public LogMaster logMaster;
        public GameObject timer;
        public GameObject boat;
        public GameObject tribeBoat;
        public GameObject partner;
        public GameObject hook;
        public GameObject eeliron;
        public GameObject orca;
        public GameObject bjørnsholm;
        public GameObject PelicanEvent;
        public GameObject spawnPoint;
    public GameObject map;
    public GameObject mapOnCam;

        //prefabs
        public GameObject torsk;
	    public GameObject eel;
	    public GameObject flatFish;
        public GameObject flint;
        public GameObject tradingObject;


        List<GameObject> caughtTotal = new List<GameObject>();
        List<GameObject> caughtEel = new List<GameObject>();
        List<GameObject> caughtTorsk =new List<GameObject>();
        List<GameObject> caughtFlatfish =new List<GameObject>();

        //tools held by guide + the paddle with audio source
        public GameObject aniTorch;
        public GameObject aniEelIron;

        // Audio Sources

        public GameObject paddle;


        //linear stuff
        bool isCountingTorsk = false;
        int startTorskAmount;
        public int currentTorskAmount=0;
        bool isCountingEel = false;
        int startEelAmount;
        public int currentEelAmount=0;
        public int currentFlatfishAmount=0;

        public bool pointingAtInteractable = false;

        public bool Islinear;
        public List<GameObject> torskArea, eelArea, flatFishArea = new List<GameObject>();
        public GameObject trading;
        public GameObject pillar1,pillar2,pillar3,pillar4,pillar5, currentPillar;

        public GameObject torskTerritory, torskTerritory2, eelTerritory, tribeTerritory;
        public GameObject basket,tribeBasket;
        public GameObject midden;

    //hidden bools for if fish has been caught in their area
    [HideInInspector]
    public bool TorskCaught = false;
    [HideInInspector]
    public bool eelCaught = false;
    [HideInInspector]
    public bool eelTrapEmptied = false;
    [HideInInspector]
    public bool flatFishCaught = false;

    //for change to end scene:
    AudioSource audio;
        bool hasFlint = false;

        //Awake is always called before any Start functions
        void Awake()
        {
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
            bjørnsholm = GameObject.FindGameObjectWithTag("bjørnholm");
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
            audio = GetComponent<AudioSource>();

            Physics.IgnoreLayerCollision(0, 11);
                
        } 

        public void AddEel(GameObject eel)
        {
            //caughtEel.Add(eel);
            caughtTotal.Add(eel);
            currentEelAmount++;
        //eelCaught = true;
            //Debug.Log("I caught an eel");
            AccumulateFish();
            if(isCountingEel)
            {
                Debug.Log(startEelAmount + " - " + currentEelAmount);
                if(currentEelAmount-startEelAmount==1)
                {
                 
                    isCountingEel=false;
                }
            }

        }
        public void AddTorsk(GameObject torsk)
        {
            //caughtTorsk.Add(torsk);
            caughtTotal.Add(torsk);
            currentTorskAmount++;
        //TorskCaught = true;
            //Debug.Log("I caught a Torsk");
            AccumulateFish();
            if(isCountingTorsk)
            {
                if(currentTorskAmount-startTorskAmount==3)
                {
                    //partner.GetComponent<PartnerSpeech>().PartnerSaysSomething(partner.GetComponent<PartnerSpeech>().AfterCodCatch, " BYT FISK FOR FLINT");
                    isCountingTorsk=false;
                }
            }
        }
        public void AddFlatFish(GameObject flat)
        {
            //caughtFlatfish.Add(flat);
            caughtTotal.Add(flat);
            currentFlatfishAmount++;
        //flatFishCaught = true;
            AccumulateFish();
        }

        public void AccumulateFish()
        {
            //commented out for debugging
            //caughtTotal.Clear();
            //caughtTotal.AddRange(caughtEel);
            //caughtTotal.AddRange(caughtTorsk);
            //caughtTotal.AddRange(caughtFlatfish);
        }

        public int GetFishCount()
        {
            return caughtTotal.Count;
        }

        //not sure about this one
        /*public void RemoveAnyFish(int amount)
        {
            for (int i = 0; i < amount; i++) 
            {
                Destroy(caughtTotal[0].gameObject);
			    caughtTotal.RemoveAt(0);

            }
        }
        public void RemoveEels(int amount)
        {
            for (int i = 0; i >amount; i++) 
            {
                try
                {
                Destroy(caughtFlatfish[0].gameObject);
			    caughtFlatfish.RemoveAt(0);
                }
                catch
                {
                }

            }
            AccumulateFish();
        }*/
        public void StartCountingTorsk()
        {
            isCountingTorsk=true;
            startTorskAmount = currentTorskAmount;
        } 
        public void StartCountingEel()
        {
            isCountingEel=true;
            startEelAmount = currentEelAmount;
        } 

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //Debug.Log("i am in end scene");
            //Debug.Log("OnSceneLoaded: " + scene.name); 
            audio.Play();
            for (int i = 1; i < currentFlatfishAmount+1; i++)
            {

                Transform[] trans = GameObject.FindGameObjectWithTag("basket").GetComponentsInChildren<Transform>(true);
                foreach (Transform t in trans) {
                    if (t.gameObject.name == "flatfish_Caught_0"+i) 
                    {
                        t.gameObject.SetActive(true);
                    }
                }
                
            }
            for (int i = 1; i < currentEelAmount+1; i++)
            {

                Transform[] trans = GameObject.FindGameObjectWithTag("basket").GetComponentsInChildren<Transform>(true);
                foreach (Transform t in trans) {
                    if (t.gameObject.name == "eel_Caught_0"+i) 
                    {
                        t.gameObject.SetActive(true);
                    }
                }
                
            }
            for (int i = 1; i < currentTorskAmount+1; i++)
            {

                Transform[] trans = GameObject.FindGameObjectWithTag("basket").GetComponentsInChildren<Transform>(true);
                foreach (Transform t in trans) {
                    if (t.gameObject.name == "torsk_Caught_0"+i) 
                    {
                        t.gameObject.SetActive(true);
                    }
                }
                
            }
            /*foreach( GameObject fish in caughtTotal)
            {
                Instantiate(fish,new Vector3(-89.575f,3.714f,162.73f), gameObject.transform.rotation);
            }
            if(hasFlint)
            {
                Instantiate(flint,new Vector3(-89.575f,3.714f,162.73f),gameObject.transform.rotation);
            }*/
        }

        public void PrepareForEndScene(AudioClip clip, bool hasFlint)
        {
            
            audio.clip = clip;
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

}
