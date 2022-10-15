using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatsManagerObject : MonoBehaviour
{
    [SerializeField] private float windowBeatTime = 0.1f;
    private float secondsPerBeat;
    private float currentLoopTime;

    private bool isPlayingMusic = false;
    private bool hasPlayedHalfBeat = false;

    private void Awake()
    {
        BeatsManager.windowBeatTime = windowBeatTime;
        MusicManager.OnPlayMusic += OnPlayMusic;
        MusicManager.OnStopMusic += OnStopMusic;
    }

    private void OnDestroy()
    {
        MusicManager.OnPlayMusic -= OnPlayMusic;
        MusicManager.OnStopMusic -= OnStopMusic;
    }

    void OnPlayMusic()
    {
        secondsPerBeat = 60.0f / BeatsManager.BPM;
        BeatsManager.secondsPerBeat = secondsPerBeat;
        currentLoopTime = secondsPerBeat;
        isPlayingMusic = true;
    }

    void OnStopMusic() 
    {
        isPlayingMusic = false;
    }

    private void Update()
    {
        if (isPlayingMusic)
        {
            currentLoopTime += Time.deltaTime;
            if (currentLoopTime >= secondsPerBeat)
            {
                BeatsManager.OnBeat?.Invoke(Time.time);
                hasPlayedHalfBeat = false;
                currentLoopTime -= secondsPerBeat;
            }
            else if (currentLoopTime >= secondsPerBeat / 2 && !hasPlayedHalfBeat)
            {
                BeatsManager.OnHalfBeat?.Invoke(Time.time);
                hasPlayedHalfBeat = true;
            }
        }
    }
}
