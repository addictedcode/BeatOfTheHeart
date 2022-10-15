using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatsManagerObject : MonoBehaviour
{
    [SerializeField] private float windowBeatTime = 0.1f;
    private WaitForSeconds secondsPerBeat;
    private WaitForSeconds secondsPerBeatWindow;

    private void Awake()
    {
        MusicManager.OnPlayMusic += OnPlayMusic;
        MusicManager.OnStopMusic += OnStopMusic;

        secondsPerBeatWindow = new WaitForSeconds(windowBeatTime / 2);
    }

    private void OnDestroy()
    {
        MusicManager.OnPlayMusic -= OnPlayMusic;
        MusicManager.OnStopMusic -= OnStopMusic;
    }

    void OnPlayMusic()
    {
        secondsPerBeat = new WaitForSeconds((60 / BeatsManager.BPM) - windowBeatTime);
        StopAllCoroutines();
        StartCoroutine(BeatLoop());
    }

    void OnStopMusic() 
    {
        StopAllCoroutines();
    }

    IEnumerator BeatLoop()
    {
        BeatsManager.OnBeat?.Invoke();
        yield return secondsPerBeatWindow;
        BeatsManager.OnAfterBeatWindow?.Invoke();
        yield return secondsPerBeat;
        while (true)
        {
            BeatsManager.OnBeforeBeatWindow?.Invoke();
            yield return secondsPerBeatWindow;
            BeatsManager.OnBeat?.Invoke();
            yield return secondsPerBeatWindow;
            BeatsManager.OnAfterBeatWindow?.Invoke();
            yield return secondsPerBeat;
        }
    }
}
