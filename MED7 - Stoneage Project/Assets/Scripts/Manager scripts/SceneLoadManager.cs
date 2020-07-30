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

        if(SceneManager.sceneCountInBuildSettings > currentIndex)
        {
            SceneManager.LoadScene(currentIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

}
