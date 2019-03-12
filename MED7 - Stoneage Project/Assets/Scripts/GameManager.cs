using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
        public static GameManager singleton = null;              //Static instance of GameManager which allows it to be accessed by any other script.                            //Current level number, expressed in game as "Day 1".

        //instances in the scene
        
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
            orca = GameObject.FindGameObjectWithTag("orca");
            //Debug.Log(orca.gameObject.name);
            PelicanEvent = GameObject.FindGameObjectWithTag("flyingPelican");
            //Debug.Log(PelicanEvent.gameObject.name);
            try{
            PelicanEvent.SetActive(false);
            }catch{}

            
            currentPillar = boat;

            //what should be turned of initially for the linear condition
            if(Islinear)
            {
                //disable the collider on following game objects
                foreach(GameObject area in torskArea)
                {
                    area.SetActive(false);
                }
                foreach(GameObject area in eelArea)
                {
                    area.SetActive(false);
                }
                foreach(GameObject area in flatFishArea)
                {
                    area.SetActive(false);
                }
                torskTerritory.GetComponent<Collider>().enabled = false;
                eelTerritory.GetComponent<Collider>().enabled = false;
                tribeTerritory.GetComponent<Collider>().enabled = false;
                //basket.GetComponent<Collider>().enabled = false;
                tribeBasket.GetComponent<Collider>().enabled = false;
                torskTerritory2.GetComponent<Collider>().enabled = false;

                //totally disable the following game objects

                tribeBasket.SetActive(false);
            }
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
        
        //Update is called every frame.
        void Update()
        {
        }

        public void AddEel(GameObject eel)
        {
            //caughtEel.Add(eel);
            caughtTotal.Add(eel);
            currentEelAmount++; 
            AccumulateFish();
            if(isCountingEel)
            {
                Debug.Log(startEelAmount + " - " + currentEelAmount);
                if(currentEelAmount-startEelAmount==1)
                {
                    partner.GetComponent<PartnerSpeech>().PartnerSaysSomething(partner.GetComponent<PartnerSpeech>().Catch2Eel, "FANG 2 ÅL");
                }
                else if(currentEelAmount-startEelAmount==2)
                {
                    partner.GetComponent<PartnerSpeech>().PartnerSaysSomething(partner.GetComponent<PartnerSpeech>().Catch1Eel, "FANG 1 ÅL");
                }
                else if(currentEelAmount-startEelAmount>=3)
                {
                    partner.GetComponent<PartnerSpeech>().PartnerSaysSomething(partner.GetComponent<PartnerSpeech>().AfterFlaringEel, "TAG TILBAGE TIL MØDDINGEN");
                    isCountingEel=false;
                }
            }

        }
        public void AddTorsk(GameObject torsk)
        {
            //caughtTorsk.Add(torsk);
            caughtTotal.Add(torsk);
            currentTorskAmount++;
            AccumulateFish();
            if(isCountingTorsk)
            {
                if(currentTorskAmount-startTorskAmount==1)
                {
                    partner.GetComponent<PartnerSpeech>().PartnerSaysSomething(partner.GetComponent<PartnerSpeech>().CodTwoMore, "FANG 2 TORSK");
                }
                else if(currentTorskAmount-startTorskAmount==2)
                {
                    partner.GetComponent<PartnerSpeech>().PartnerSaysSomething(partner.GetComponent<PartnerSpeech>().CodOneMore, "FANG 1 TORSK");
                }
                else if(currentTorskAmount-startTorskAmount==3)
                {
                    partner.GetComponent<PartnerSpeech>().PartnerSaysSomething(partner.GetComponent<PartnerSpeech>().AfterCodCatch, " BYT FISK FOR FLINT");
                    isCountingTorsk=false;
                }
            }
        }
        public void AddFlatFish(GameObject flat)
        {
            //caughtFlatfish.Add(flat);
            caughtTotal.Add(flat);
            currentFlatfishAmount++;
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
        public void RemoveAnyFish(int amount)
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
        }
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
            /*Debug.Log("i am in end scene");
            Debug.Log("OnSceneLoaded: " + scene.name); */
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

}
