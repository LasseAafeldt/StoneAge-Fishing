using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private bool _readyForNextScene = false;

    [SerializeField] private VideoControls vidControls;
    private void Start()
    {
        if(vidControls != null)
            vidControls.VideoHasEnded += OnVideoEnd;
    }
    public bool ReadyForNextScene {
        get { return _readyForNextScene; }
        private set{_readyForNextScene = value;}
    }

    /// <summary>
    /// Load scene with specific index.
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void ChangeScene(int sceneIndex, float delay= 0)
    {
        StartCoroutine(LoadScene(sceneIndex, delay));
    }

    /// <summary>
    /// Load the next scene from the build index.
    /// </summary>
    public void ChangeScene(float delay = 0)
    {
        int loadIndex;

        if(SceneManager.sceneCountInBuildSettings > CurrentSceneIndex())
        {
            loadIndex = CurrentSceneIndex() + 1;
        }
        else
        {

            loadIndex = 0;
        }
        StartCoroutine(LoadScene(loadIndex, delay));
    }

    public void ChangeSceneAsync()
    {
        int currentScene = CurrentSceneIndex();
        //Debug.Log("Scene index = " + currentScene);
        int sceneIndexToLoad = currentScene +1;

        //Debug.Log("Scene count = " + SceneManager.sceneCountInBuildSettings);
        if(sceneIndexToLoad > SceneManager.sceneCountInBuildSettings-1)
        {
            sceneIndexToLoad = 0;
        }
        //Debug.Log("Index to load = " + sceneIndexToLoad);
        StartCoroutine(AsyncLoadScene(sceneIndexToLoad));
    }

    IEnumerator LoadScene(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        FadeController fade = GameObject.FindObjectOfType<FadeController>();
        fade.fadeOut();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(index);
    }

    IEnumerator AsyncLoadScene(int index)
    {
        FadeController fade = GameObject.FindObjectOfType<FadeController>();
        fade.fadeOut();
        yield return new WaitForSeconds(2f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);

        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            if(!ReadyForNextScene)
                Debug.Log("<color=red> Ready for next scene = " + ReadyForNextScene + "</color>");
            else
                Debug.Log("<color=green> Ready for next scene = " + ReadyForNextScene + "</color>");

            //wait for ready signal
            if (ReadyForNextScene)
            {
                asyncLoad.allowSceneActivation = true;
                //Debug.Log("<color=blue> Should activate </color>");
            }
            yield return null;
        }
    }

    private int CurrentSceneIndex()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        return currentIndex;
    }

    private void OnVideoEnd()
    {
        Debug.Log("<color=green> Vidoe has ended </color>");
        ReadyForNextScene = true;
    }

    private void OnDisable()
    {
        VideoControls vidControls = GameObject.FindObjectOfType<VideoControls>();
        //Debug.Log("We have the vid controller:     " + vidControls.name);
        if (vidControls != null)
            vidControls.VideoHasEnded -= OnVideoEnd;
    }
}
