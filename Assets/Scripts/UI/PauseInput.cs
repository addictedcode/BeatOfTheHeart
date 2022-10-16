using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseInput : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField][Scene] private string settingsScene;

    public void OnCancel()
    {
        if (SceneManager.GetSceneByName(settingsScene).isLoaded)
            return;
        pauseCanvas.SetActive(!pauseCanvas.activeSelf);
        Time.timeScale = pauseCanvas.activeSelf ? 0.0f : 1.0f;
        MusicManager.player.PauseMusic(pauseCanvas.activeSelf);
    }

    public void OnSettingsPress()
    {
        SceneManager.LoadSceneAsync(settingsScene, LoadSceneMode.Additive);
    }

    public void OnReturnToMainMenuPress()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
}
