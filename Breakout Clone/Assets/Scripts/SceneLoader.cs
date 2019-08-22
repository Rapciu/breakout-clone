using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int newSceneIndex = currentSceneIndex + 1;

        if (newSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            newSceneIndex = 1;
        }

        SceneManager.LoadScene(newSceneIndex);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
