using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivityManager : MonoBehaviour, IHandleActivity
{
    [SerializeField] private int queSizeSeconds = 15;
    [SerializeField] private int checksPerSecond = 3;
    [SerializeField] private bool defaultQueValue; //false
    [SerializeField] private bool triggerOnActivity; //true

    private bool canChangeScene= true;

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
        playerIsActive = defaultQueValue;
    }

    private void FixedUpdate()
    {
        if(!playerIsActive && !triggerOnActivity)
        {
            OnEnterPause();
        }
        else if(playerIsActive && triggerOnActivity)
        {
            ONExitPause();
        }
    }
    public void OnEnterPause()
    {
        if (SceneManager.GetActiveScene().buildIndex != SceneManager.sceneCountInBuildSettings - 1 &&
            canChangeScene)
        {
            sceneLoad.ChangeScene(SceneManager.sceneCountInBuildSettings - 1);
            canChangeScene = false;
        }
    }

    public void ONExitPause()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0
            && canChangeScene)
        {
            sceneLoad.ChangeScene(0);
            canChangeScene = false;
        }

    }

    void CheckIsActive()
    {
        tracker.UpdateTracking(queSizeSeconds, checksPerSecond, container);
        Debug.Log("Counter");
        playerIsActive = tracker.GetIsActive(container.activityQue);
    }


}
