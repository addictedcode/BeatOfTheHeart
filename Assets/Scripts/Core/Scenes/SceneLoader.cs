using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static string currentScene;
    public static AsyncOperation currentOp;
    public static Action onStartSceneLoad;

    public static void LoadSceneWithLoadingBar(string scene, bool shouldWait = false)
    {
        currentScene = scene;
        _ = LoadLoadingBarAsync(shouldWait);
    }

    static async Task LoadLoadingBarAsync(bool shouldWait)
    {
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
        await Task.Delay(5);
        currentOp = SceneManager.LoadSceneAsync(currentScene, LoadSceneMode.Additive);
        currentOp.allowSceneActivation = false;
        onStartSceneLoad.Invoke();

        if(!shouldWait)
            currentOp.allowSceneActivation = true;

        while (!currentOp.isDone)
        {
            await Task.Delay(10);
        }

        SceneManager.UnloadSceneAsync("LoadingScene");
    }
}
