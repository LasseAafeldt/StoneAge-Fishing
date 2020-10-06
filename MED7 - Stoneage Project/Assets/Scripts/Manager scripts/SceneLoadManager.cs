using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{

    /// <summary>
    /// Load scene with specific index.
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    /// <summary>
    /// Load the next scene from the build index.
    /// </summary>
    public void ChangeScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int loadIndex;

        if(SceneManager.sceneCountInBuildSettings > currentIndex)
        {
            loadIndex = currentIndex + 1;
        }
        else
        {

            loadIndex = 0;
        }
        StartCoroutine(LoadScene(loadIndex));
        //SceneManager.LoadScene(loadIndex);
    }

    IEnumerator LoadScene(int index)
    {
        FadeController fade = GameObject.FindObjectOfType<FadeController>();
        fade.fadeOut();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(index);
    }

    //yield return new WaitForSeconds(clip.length);
    //FadeController fade = GameObject.FindObjectOfType<FadeController>();
    //fade.fadeOut();
    //    yield return new WaitForSeconds(2f);
    //EC.CheckForEnding();


}
