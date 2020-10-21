using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToStart : MonoBehaviour {

	public float totalTime =10;
    float tempTime;

    SceneLoadManager sceneManager;

    private void Awake()
    {
        sceneManager = GetComponent<SceneLoadManager>();
    }

    void Start () {
        tempTime = totalTime;
	}
	
	void Update () {
		tempTime -= Time.deltaTime;

		if(tempTime < 0 && Input.GetButtonDown("Fire1"))
		{
            GameManager.singleton.resetFishInBasket();
            sceneManager.ChangeScene(0);
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
