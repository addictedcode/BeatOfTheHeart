using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField][Scene] private string mainMenuScene;

    public void OnBackPress()
    {
        SceneLoader.UnloadScene(gameObject.scene.name);
        SceneLoader.LoadScene(mainMenuScene);
    }
}
