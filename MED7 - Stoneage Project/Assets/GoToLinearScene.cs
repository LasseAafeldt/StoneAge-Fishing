using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToLinearScene : MonoBehaviour {

	 public Text loading;
    LogMaster logMaster;

    // Use this for initialization
    void Start () {

        logMaster = FindObjectOfType<LogMaster>();
        Debug.Log(logMaster);
        loading.text="";
    }
	
    public void LoadScene()
    {
            loading.text="Indlæser...";
        logMaster.createFile();
        StartCoroutine(delayLoadScene());
    }

    IEnumerator delayLoadScene()
    {
        yield return new WaitForSeconds(0.5f);
        LogMaster.shouldBeLogging = true;
        SceneManager.LoadScene("mainScene", LoadSceneMode.Single);
    }
}
