using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToLinearScene : MonoBehaviour {

	 public Text loading;
    LogMaster logMaster;

    private void Awake()
    {
        if(GameObject.Find("Game Manager") != null)
        {
            Destroy(GameObject.Find("Game Manager"));
        }
    }

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
        FadeController fade = GameObject.FindObjectOfType<FadeController>();
        fade.fadeOut();
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(0.5f);
        LogMaster.shouldBeLogging = true;
        SceneManager.LoadScene("mainScene", LoadSceneMode.Single);
    }
}
