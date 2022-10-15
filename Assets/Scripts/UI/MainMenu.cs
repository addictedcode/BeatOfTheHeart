using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField][Scene] private string settingsScene;

    public void OnStartPress()
    {

    }

    public void OnSettingsPress()
    {
        SceneLoader.UnloadScene(gameObject.scene.name);
        SceneLoader.LoadScene(settingsScene);
    }

    public void OnQuitPress()
    {
        Application.Quit();
    }
}
