using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    [SerializeField] private GameObject pressAnyKeyToContinueText;
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
            if (sceneOp.progress >= 0.9f && !sceneOp.allowSceneActivation)
            {
                pressAnyKeyToContinueText.SetActive(true);
            }
            float progress = Mathf.Clamp01(sceneOp.progress / 0.9f);
            loadingBar.value = progress;
            if (Input.anyKeyDown && !sceneOp.allowSceneActivation)
            {
                sceneOp.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
