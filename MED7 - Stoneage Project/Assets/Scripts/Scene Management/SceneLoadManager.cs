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
        int sceneIndexToLoad = currentScene +1;

        if(sceneIndexToLoad > SceneManager.sceneCountInBuildSettings-1)
        {
            sceneIndexToLoad = 0;
        }
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
            //wait for ready signal
            if (ReadyForNextScene)
            {
                asyncLoad.allowSceneActivation = true;
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
        ReadyForNextScene = true;
    }

    private void OnDisable()
    {
        VideoControls vidControls = GameObject.FindObjectOfType<VideoControls>();
        if (vidControls != null)
            vidControls.VideoHasEnded -= OnVideoEnd;
    }
}
