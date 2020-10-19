using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivityManager : MonoBehaviour, IHandleActivity
{
    [SerializeField] private int queSizeSeconds = 15;
    [SerializeField] private int checksPerSecond = 3;

    private ITrackActivity tracker;
    private bool playerIsActive;
    private SceneLoadManager sceneLoad;

    public static ActivityManager instance = null;

    private void Awake()
    {
        //make sure ther are only one of these in a scene as the values of the que 
        //would be wrong double logged otherwise
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        tracker = GetComponent<ITrackActivity>();
        ActivityQueContainer.InstantiateQue(queSizeSeconds * checksPerSecond);
        sceneLoad = new SceneLoadManager();
    }

    private void Start()
    {
        InvokeRepeating("CheckIsActive", 1, 1 / checksPerSecond);
        
    }

    private void FixedUpdate()
    {
        if(!playerIsActive && SceneManager.GetActiveScene().buildIndex != 
            SceneManager.sceneCountInBuildSettings - 1)
        {
            OnEnterPause();
        }
        else if(playerIsActive && SceneManager.GetActiveScene().buildIndex == 
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

    void CheckIsActive()
    {
        tracker.UpdateTracking(queSizeSeconds, checksPerSecond);
        playerIsActive = tracker.GetIsActive();
    }
}
