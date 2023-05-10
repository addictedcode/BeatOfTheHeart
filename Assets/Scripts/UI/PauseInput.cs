using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseInput : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private CanvasGroup resumePrompt;
    [SerializeField][Scene] private string settingsScene;
    [SerializeField] private Animator pauseAnimator;
    [SerializeField] private TMP_Text beatText;
    [SerializeField] private bool enableResumePrompt;

    private bool isAnimating;
    private bool isWaitingForInput;
    private bool willWaitForInput;

    private void Update()
    {
        if (isWaitingForInput && Input.anyKeyDown)
        {
            pauseAnimator.SetTrigger("Exit");
            
            AE_OnPauseEnd();

            StartCoroutine(DelayWaitingForInputDisable());
                
            return;
        }
    }

    public void OnCancel()
    {
        // Debug for Count-down
        /*if (isAnimating && pauseCanvas.activeSelf)
        {
            pauseAnimator.SetTrigger("Beat");
            
            return;
        }*/
        
        if (SceneManager.GetSceneByName(settingsScene).isLoaded || willWaitForInput || GameManager.Instance.GameState == GameState.GameOver)
            return;
        
        if (!pauseCanvas.activeSelf)
            PauseOpen();
        else
            PauseClose();
    }

    public void PauseOpen()
    {
        GameManager.Instance.GameState = GameState.Paused;
        GameManager.Instance.PlayerInput.enabled = false;
        pauseCanvas.SetActive(true);
        resumePrompt.gameObject.SetActive(true);
        Time.timeScale = 0.0f;
        MusicManager.player.PauseMusic(true);
    }

    public void PauseClose()
    {
        isAnimating = true;
        pauseAnimator.SetTrigger("Close");
        pauseAnimator.SetBool("PromptOn", enableResumePrompt);
        willWaitForInput = enableResumePrompt;
    }

    public void AE_OnStartBeat()
    {
        pauseAnimator.SetTrigger("Beat");
    }
    
    public void AE_OnPauseEnd()
    {
        GameManager.Instance.GameState = GameState.Playing;
        GameManager.Instance.PlayerInput.enabled = true;
        isAnimating = false;
        pauseCanvas.SetActive(false);
        resumePrompt.gameObject.SetActive(false);
        resumePrompt.alpha = 0;
        Time.timeScale = 1.0f;
        MusicManager.player.PauseMusic(false);
        pauseAnimator.SetTrigger("Exit");
    }
    
    public void AE_OnBeat(int value)
    {
        beatText.text = value.ToString();
    }

    public void AE_OnThirdBeat()
    {
        // Calculate for the time
        Time.timeScale = 0.1f;
    }

    public void AE_StartWaitingForInput()
    {
        isWaitingForInput = true;
    }

    public void OnSettingsPress()
    {
        SceneManager.LoadSceneAsync(settingsScene, LoadSceneMode.Additive);
    }

    public void OnReturnToMainMenuPress()
    {
        Destroy(SFXManager.Instance.gameObject);
        Destroy(GameManager.Instance.gameObject);
        MusicManager.OnStopMusic.Invoke();
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
    
    IEnumerator DelayWaitingForInputDisable()
    {
        yield return null;

        willWaitForInput = false;
        isWaitingForInput = false;
    }
}