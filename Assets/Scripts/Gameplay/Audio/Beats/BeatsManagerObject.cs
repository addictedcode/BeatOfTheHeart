using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatsManagerObject : MonoBehaviour
{
    [SerializeField] private float windowBeatTime = 0.1f;
    [SerializeField] private float offsetBeatTime = 0.1f;
    private float secondsPerBeat;
    private float currentLoopTime;

    private bool isPlayingMusic = false;

    private void Awake()
    {
        BeatsManager.windowBeatTime = windowBeatTime;
        BeatsManager.offsetBeatTime = offsetBeatTime;
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
                currentLoopTime -= secondsPerBeat;
            }
        }
    }
}
