using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToStart : MonoBehaviour {

	public float totalTime =10;
    float tempTime;

    ISceneManager sceneManager;

    private void Awake()
    {
        sceneManager = GetComponent<ISceneManager>();
    }

    void Start () {
        tempTime = totalTime;
        //gameManager = GameManager.singleton;
	}
	
	// Update is called once per frame
	void Update () {
		tempTime -= Time.deltaTime;

		if(tempTime < 0 && Input.GetButtonDown("Fire1"))
		{
			Debug.Log("go to start");
            GameManager.singleton.resetFishInBasket();
            //LogMaster.shouldBeLogging = true; this enables it too early
            //StartCoroutine(waitWithDestroy(2f));
            //Destroy(GameObject.Find("Game Manager"));
            sceneManager.ChangeScene(0);  //StartCoroutine(waitWithDestroy(2f));
		}
	}
    IEnumerator waitWithDestroy(float wait)
    {
        FadeController fade = GameObject.FindObjectOfType<FadeController>();
        fade.fadeOut();
        yield return new WaitForSeconds(wait);
		Destroy(GameObject.Find("Game Manager"));
        SceneManager.LoadScene("start", LoadSceneMode.Single);

    }
}
