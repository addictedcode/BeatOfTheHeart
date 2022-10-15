using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatsManagerObject : MonoBehaviour
{
    [SerializeField] private float windowBeatTime = 0.1f;
    [SerializeField] private float offsetBeatTime = 0.1f;
    private float secondsPerBeat;
    private float secondsPerBeatWindow;

    private float currentMusicDuration = 0.0f;
    private float currentLoopTime = 0.0f;
    private bool isPlayingMusic = false;

    enum BeatWindow
    {
        OnBeforeBeatWindow = 1,
        OnAfterBeatWindow = 2,
        NotOnBeatWindow = 3
    }

    private BeatWindow window = BeatWindow.NotOnBeatWindow;

    private void Awake()
    {
        MusicManager.OnPlayMusic += OnPlayMusic;
        MusicManager.OnStopMusic += OnStopMusic;

        secondsPerBeatWindow = windowBeatTime / 2.0f;
    }

    private void OnDestroy()
    {
        MusicManager.OnPlayMusic -= OnPlayMusic;
        MusicManager.OnStopMusic -= OnStopMusic;
    }

    void OnPlayMusic()
    {
        secondsPerBeat = 60.0f / BeatsManager.BPM;
        window = BeatWindow.OnBeforeBeatWindow;
        currentMusicDuration = -offsetBeatTime;
        isPlayingMusic = true;
        
    }

    void OnStopMusic() 
    {
        isPlayingMusic = false;
        currentMusicDuration = 0.0f;
    }

    private void Update()
    {
        if (isPlayingMusic)
        {
            currentMusicDuration += Time.deltaTime;
            currentLoopTime = currentMusicDuration % secondsPerBeat;
            if (window == BeatWindow.NotOnBeatWindow)
            {
                if (currentLoopTime > secondsPerBeat - secondsPerBeatWindow)
                {
                    BeatsManager.OnBeforeBeatWindow?.Invoke();
                    window = BeatWindow.OnBeforeBeatWindow;
                }
            }
            else if (window == BeatWindow.OnBeforeBeatWindow)
            {
                if (currentLoopTime > 0.0f && currentLoopTime < secondsPerBeatWindow)
                {
                    BeatsManager.OnBeat?.Invoke();
                    window = BeatWindow.OnAfterBeatWindow;
                }
            }
            else if (window == BeatWindow.OnAfterBeatWindow)
            {
                if (currentLoopTime > secondsPerBeatWindow)
                {
                    BeatsManager.OnAfterBeatWindow?.Invoke();
                    window = BeatWindow.NotOnBeatWindow;
                }
            }
        }
    }
}
