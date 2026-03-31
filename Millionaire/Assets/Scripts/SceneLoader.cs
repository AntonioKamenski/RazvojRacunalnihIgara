using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadNextScene()
    {
        int currentSceneID = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneID + 1);
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene(0);
    }

    static public void LoadSceneByName(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void QuitRequest()
    {
        Application.Quit();
    }


}
