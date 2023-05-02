using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    [SerializeField] private GameObject pressAnyKeyToContinueText;
    private Slider loadingBar;

    private void Awake()
    {
        loadingBar = GetComponent<Slider>();
    }

    private void Start()
    {
        SceneLoader.onStartSceneLoad += LoadingBarUpdate;
    }

    private void OnDisable()
    {
        SceneLoader.onStartSceneLoad -= LoadingBarUpdate;
    }

    private void LoadingBarUpdate()
    {
        StartCoroutine(UpdateLoadingBar());
    }

    IEnumerator UpdateLoadingBar() 
    {
        while (!SceneLoader.CheckCurrentOpsIsDone())
        {
            if (SceneLoader.GetAverageProgress() >= 0.89f && !SceneLoader.CheckAllowSceneActivation())
            {
                pressAnyKeyToContinueText.SetActive(true);
            }
            float progress = Mathf.Clamp01(SceneLoader.GetAverageProgress() / 0.9f);
            loadingBar.value = progress;
            if (Input.anyKeyDown && !SceneLoader.CheckAllowSceneActivation())
            {
                SceneLoader.SetAllowSceneActivation(true);
            }
            yield return null;
        }
    }
}
