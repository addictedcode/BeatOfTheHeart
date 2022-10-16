using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Stage gameStage;
    [SerializeField][Scene] private string settingsScene;

    [SerializeField] private MusicFile mainMenuMusic;

    private void Start()
    {
        MusicManager.player.PlayMusic(mainMenuMusic);
    }

    public void OnStartPress()
    {
        SceneLoader.onFinishSceneLoad += FinishSceneLoad;
        SceneLoader.LoadScenesWithLoadingBar(gameStage.gameScene, true);
        SceneManager.UnloadSceneAsync(gameObject.scene);
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
