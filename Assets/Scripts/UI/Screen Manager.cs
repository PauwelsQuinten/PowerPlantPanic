using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    [SerializeField]
    private List<SceneAsset> _scenesAssets;

    private List<Scene> _scenes = new List<Scene>();

    private void Start()
    {
        int sceneAssetIndex = 0;
        //Transform all the sceneAssets to scenes
        foreach (var item in _scenesAssets)
        {
            _scenes.Add(SceneManager.GetSceneByName(_scenesAssets[sceneAssetIndex].name));
            sceneAssetIndex++;
        }
    }

    public void StartGame() //Next scene is the main game
    {
        int nextSceneIndex;

        nextSceneIndex = _scenes.IndexOf(SceneManager.GetActiveScene()) + 1;

        SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Single);
        //SceneManager.SetActiveScene(_scenes[nextSceneIndex]);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Credits()
    {
        SceneManager.LoadSceneAsync(_scenes.Count -1, LoadSceneMode.Single);
        //SceneManager.SetActiveScene(_scenes[_scenes.Count-1]);
    }

}
