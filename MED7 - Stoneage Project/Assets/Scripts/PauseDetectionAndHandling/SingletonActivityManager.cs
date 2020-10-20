using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonActivityManager : MonoBehaviour, IHandleActivity
{
    [SerializeField] private int queSizeSeconds = 15;
    [SerializeField] private int checksPerSecond = 3;
    [SerializeField] private bool defaultQueValue;
    [SerializeField] private bool triggerOnActivity;

    private ActivityQueContainer container;

    private ITrackActivity tracker;
    private bool playerIsActive;
    private SceneLoadManager sceneLoad;

    private void Awake()
    {
        container = new ActivityQueContainer();
        container.InstantiateQue(queSizeSeconds * checksPerSecond, defaultQueValue);

        
        tracker = GetComponent<ITrackActivity>();
        sceneLoad = FindObjectOfType<SceneLoadManager>();
    }

    private void Start()
    {
        InvokeRepeating("CheckIsActive", 1, 1f / checksPerSecond);
        playerIsActive = true;
    }

    private void FixedUpdate()
    {
        if (!playerIsActive && SceneManager.GetActiveScene().buildIndex !=
            SceneManager.sceneCountInBuildSettings - 1)
        {
            Debug.Log("indext to load = " + (SceneManager.sceneCountInBuildSettings - 1));
            OnEnterPause();
        }
        else if (playerIsActive && SceneManager.GetActiveScene().buildIndex ==
            SceneManager.sceneCountInBuildSettings - 1)
        {
            ONExitPause();
        }
    }
    public void OnEnterPause()
    {
        sceneLoad.ChangeScene(SceneManager.sceneCountInBuildSettings - 1);
    }

    public void ONExitPause()
    {
        sceneLoad.ChangeScene(0);
    }

    //void CheckIsActive()
    //{
    //    tracker.UpdateTracking(queSizeSeconds, checksPerSecond, );
    //    Debug.Log("Counter");
    //    playerIsActive = tracker.GetIsActive();
    //}


}
