using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Stage gameStage;
    [SerializeField][Scene] private string settingsScene;

    [SerializeField] private MusicFile mainMenuMusic;

    [SerializeField] private CinemachineVirtualCamera mainMenuVC;

    private void Start()
    {
        MusicManager.player.PlayMusic(mainMenuMusic);
    }

    public void OnStartPress()
    {
        
        //SceneLoader.onFinishSceneLoad += FinishSceneLoad;
        SceneManager.UnloadSceneAsync(gameObject.scene);
        SceneLoader.LoadScenes(gameStage.gameScene, true);

        MusicManager.player.StopMusic();
        MusicManager.player.PlayMusicAfterDelay(gameStage.musicFile, gameStage.timeBeforeGameActuallyStarts);
        GameManager.Instance.StartGameplay(gameStage.timeBeforeGameActuallyStarts, mainMenuVC);

    }

    private void FinishSceneLoad()
    {
        MusicManager.player.StopMusic();
        MusicManager.player.PlayMusicAfterDelay(gameStage.musicFile, gameStage.timeBeforeGameActuallyStarts);
        GameManager.Instance.PlayGameAfterDelay(gameStage.timeBeforeGameActuallyStarts);

        SceneLoader.onFinishSceneLoad -= FinishSceneLoad;
    }

    public void OnSettingsPress()
    {
        SceneManager.LoadSceneAsync(settingsScene, LoadSceneMode.Additive);
    }

    public void OnQuitPress()
    {
        Application.Quit();
    }
}
