using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    private int _scenes;

    private void Start()
    {
        _scenes = SceneManager.sceneCountInBuildSettings;
    }

    public void StartGame() //Next scene is the main game
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Credits()
    {
        SceneManager.LoadSceneAsync(_scenes -1, LoadSceneMode.Single);
    }

    public void StartMenu()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
}
