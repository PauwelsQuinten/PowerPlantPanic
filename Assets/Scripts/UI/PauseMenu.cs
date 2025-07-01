using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenu;

    [SerializeField]
    private InputAction _pauseButton;

    private int _scenes;

    private void OnEnable()
    {
        _scenes = SceneManager.sceneCountInBuildSettings;

        _pauseButton.Enable();
        _pauseButton.started += _pauseButton_started;
        _pauseMenu.SetActive(false);
    }

    private void OnDisable()
    {
        _pauseButton.started -= _pauseButton_started;
    }

    private void _pauseButton_started(InputAction.CallbackContext obj)
    {
        Time.timeScale = 0f;
        _pauseMenu.SetActive(true);
    }

    public void Credits()
    {
        SceneManager.LoadSceneAsync(_scenes - 1, LoadSceneMode.Single);
        Time.timeScale = 1f;
    }

    public void StartMenu()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        Time.timeScale = 1f;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        _pauseMenu.SetActive(false);
    }
}
