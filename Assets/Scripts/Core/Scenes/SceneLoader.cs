using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static List<string> currentScenes;
    public static List<AsyncOperation> currentOps = new();
    public static Action onStartSceneLoad;
    public static Action onFinishSceneLoad;

    public static void LoadSceneWithLoadingBar(string scene, bool shouldWait = false)
    {
        currentScenes = new();
        currentScenes.Add(scene);
        _ = LoadLoadingBarAsync(shouldWait);
    }
    public static void LoadScenesWithLoadingBar(List<string> scenes, bool shouldWait = false)
    {
        currentScenes = scenes;
        _ = LoadLoadingBarAsync(shouldWait);
    }

    public static void LoadScenes(List<string> scenes, bool shouldWait)
    {
        currentScenes = scenes;
        _ = LoadAsync(shouldWait);
    }

    static async Task LoadLoadingBarAsync(bool shouldWait)
    {
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
        await Task.Delay(5);
        currentOps.Clear();
        foreach (var scene in currentScenes)
        {
            var currentOp = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            currentOp.allowSceneActivation = false;
            currentOps.Add(currentOp);
        }
        onStartSceneLoad.Invoke();

        if (!shouldWait)
        {
            SetAllowSceneActivation(true);
        }

        while (!CheckCurrentOpsIsDone())
        {
            await Task.Delay(10);
        }

        onFinishSceneLoad?.Invoke();
        SceneManager.UnloadSceneAsync("LoadingScene");
    }

    static async Task LoadAsync(bool shouldWait)
    {
        currentOps.Clear();
        foreach (var scene in currentScenes)
        {
            var currentOp = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            currentOp.allowSceneActivation = false;
            currentOps.Add(currentOp);
        }
        onStartSceneLoad.Invoke();

        if (!shouldWait)
        {
            SetAllowSceneActivation(true);
        }

        while (!CheckCurrentOpsIsDone())
        {
            await Task.Delay(10);
        }

        onFinishSceneLoad?.Invoke();
    }

    public static bool CheckCurrentOpsIsDone()
    {
        foreach (AsyncOperation op in currentOps)
        {
            if (!op.isDone)
            {
                return false;
            }
        }
        return true;
    }

    public static bool CheckAllowSceneActivation()
    {
        foreach (AsyncOperation op in currentOps)
        {
            if (!op.allowSceneActivation)
            {
                return false;
            }
        }
        return true;
    }

    public static float GetAverageProgress()
    {
        float avgProgress = 0.0f;
        foreach (AsyncOperation op in currentOps)
        {
            avgProgress += op.progress;
        }
        return avgProgress /= currentOps.Count;
    }

    public static void SetAllowSceneActivation(bool value)
    {
        foreach (var op in currentOps)
        {
            op.allowSceneActivation = value;
        }
    }
}
