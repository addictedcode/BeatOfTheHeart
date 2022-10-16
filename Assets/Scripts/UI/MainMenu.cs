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
        SceneLoader.onFinishSceneLoad += () => {
            MusicManager.player.StopMusic();
            MusicManager.player.PlayMusicAfterDelay(gameStage.musicFile, gameStage.timeBeforeGameActuallyStarts); };
        SceneLoader.LoadScenesWithLoadingBar(gameStage.gameScene, true);
        SceneManager.UnloadSceneAsync(gameObject.scene);
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
