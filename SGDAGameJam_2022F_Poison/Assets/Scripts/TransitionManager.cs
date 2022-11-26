using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance = null;

    void Awake()
    {
        if (TransitionManager.Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void TransitionToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void QuitGame()
    {
        Application.Quit();
    }
}
