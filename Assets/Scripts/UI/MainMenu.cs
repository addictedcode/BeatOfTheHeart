using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Stage gameStage;
    [SerializeField][Scene] private string settingsScene;

    public void OnStartPress()
    {
        SceneLoader.onFinishSceneLoad += () => { MusicManager.player.PlayMusic(gameStage.musicFile); };
        SceneLoader.LoadSceneWithLoadingBar(gameStage.gameScene, true);
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }

    public void OnSettingsPress()
    {
        SceneManager.LoadSceneAsync(settingsScene, LoadSceneMode.Additive); ;
    }

    public void OnQuitPress()
    {
        Application.Quit();
    }
}
