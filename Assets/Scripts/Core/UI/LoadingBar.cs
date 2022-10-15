using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    private AsyncOperation sceneOp;
    private Slider loadingBar;

    private void Awake()
    {
        loadingBar = GetComponent<Slider>();
    }

    private void Start()
    {
        SceneLoader.onStartSceneLoad += () =>
        {
            sceneOp = SceneLoader.currentOp;
            StartCoroutine(UpdateLoadingBar());
        };
    }

    IEnumerator UpdateLoadingBar() 
    {
        while (!sceneOp.isDone)
        {
            float progress = Mathf.Clamp01(sceneOp.progress / 0.9f);
            loadingBar.value = progress;
            yield return null;
        }
    }
}
