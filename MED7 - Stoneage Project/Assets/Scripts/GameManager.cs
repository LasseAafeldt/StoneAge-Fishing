﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    // DISCLAIMER!!! catching fish is handled in PartnerAnimator  -keywords: fishcaught, fish caught, 

    //Static instance of GameManager which allows it to be accessed by any other script.                            
    public static GameManager singleton = null;

    [HideInInspector]
    public bool canMove = true;

    public bool useGuidanceSounds = true;

    [SerializeField] private int _fishWinamount;
    public int FishWinAmount { get { return _fishWinamount; } }

    //instances in the scene
    [Header("Object references to be used by other scripts")]
    [HideInInspector]
    public GameObject CameraContainer;
    [HideInInspector]
    public GameObject timer;
    [HideInInspector]
    public GameObject boat;

    public GameObject partner;

    [HideInInspector]
    public GameObject hook;
    [HideInInspector]
    public GameObject eeliron;
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

    [HideInInspector]
    public bool pointingAtInteractable = false;
    // Audio Sources
    [HideInInspector]
    public AudioSource _audio;

    //hidden bools for if fish has been caught in their area
    [HideInInspector]
    public bool TorskCaught = false;
    [HideInInspector]
    public bool eelCaught = false;
    [HideInInspector]
    public bool eelTrapEmptied = false;
    [HideInInspector]
    public bool flatFishCaught = false;

    private int caughtTotal = 0;
    private int caughtEel = 0;
    private int caughtTorsk = 0;
    private int caughtFlatfish = 0;
    
    //for change to end scene:
    private bool hasFlint = false;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (singleton == null)
                
            //if not, set instance to this
            singleton = this;

        else if (singleton != null && SceneManager.GetActiveScene().name == "mainScene" ) // if we are in the main scene we want to replace the current singleton with the main scene's game manager
        {

            Destroy(singleton.gameObject);
            singleton = this;
        }
        //If instance already exists and it's not this:
        else if (singleton != this)
                
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);    
            
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        CameraContainer = GameObject.FindGameObjectWithTag("Guidance");

        timer = GameObject.FindGameObjectWithTag("timer");
        boat = GameObject.FindGameObjectWithTag("boat");        
        partner = GameObject.FindGameObjectWithTag("partner");
        hook = GameObject.FindGameObjectWithTag("hook");
        eeliron = GameObject.FindGameObjectWithTag("eeliron");
        paddle = GameObject.FindGameObjectWithTag("paddle");
        PelicanEvent = GameObject.FindGameObjectWithTag("flyingPelican");
        try
        {
            PelicanEvent.SetActive(false);
        }catch{}

        _audio = GetComponent<AudioSource>();

        Physics.IgnoreLayerCollision(0, 11);
                
    }
    private void Start()
    {
        if(!useGuidanceSounds)
        {
            StartCoroutine(makeSureStartSoundPlays(guide.StartingSoundGoFishing));
        }
    }

    public void AddEel(int _amount)
    {
        caughtEel+= _amount;
    }
    public int getEelAmount()
    {
        return caughtEel;
    }

    public void AddTorsk(int _amount)
    {
        caughtTorsk += _amount;
    }
    public int getTorskAmount()
    {
        return caughtTorsk;
    }

    public void AddFlatFish(int _amount)
    {
        caughtFlatfish += _amount;
    }
    public int getFlatfishAmount()
    {
        return caughtFlatfish;
    }

    public int GetFishCount()
    {
        caughtTotal = caughtTorsk+caughtFlatfish+caughtEel;
        return caughtTotal;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "End Scene")
        {
            for (int i = 1; i <= caughtFlatfish; i++)
            {
                Transform[] trans = GameObject.FindGameObjectWithTag("basket").GetComponentsInChildren<Transform>(true);
                foreach (Transform t in trans)
                {
                    if (t.gameObject.name == "flatfish_Caught_0" + i)
                    {
                        t.gameObject.SetActive(true);
                    }
                }
            }
            for (int i = 1; i <= caughtEel; i++)
            {
                Transform[] trans = GameObject.FindGameObjectWithTag("basket").GetComponentsInChildren<Transform>(true);
                foreach (Transform t in trans)
                {
                    if (t.gameObject.name == "eel_Caught_0" + i)
                    {
                        t.gameObject.SetActive(true);
                    }
                }
            }
            for (int i = 1; i <= caughtTorsk; i++)
            {
                Transform[] trans = GameObject.FindGameObjectWithTag("basket").GetComponentsInChildren<Transform>(true);
                foreach (Transform t in trans)
                {
                    if (t.gameObject.name == "torsk_Caught_0" + i)
                    {
                        t.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void PrepareForEndScene(AudioClip clip, bool hasFlint)
    {
            
        _audio.clip = clip;
        this.hasFlint = hasFlint;
    }

    IEnumerator makeSureStartSoundPlays(AudioClip clip)
    {
        yield return new WaitForSeconds(clip.length);
        CameraContainer.GetComponent<GuidanceSounds>().enabled = false;
    }

    public void resetFishInBasket()
    {
        caughtEel = 0;
        caughtFlatfish = 0;
        caughtTorsk = 0;
        caughtTotal = 0;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

}
