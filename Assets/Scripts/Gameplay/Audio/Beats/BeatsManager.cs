using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BeatsManager
{
    public static uint BPM = 120;
    public static float secondsPerBeat = 0.5f;
    public static float windowBeatTime = 0.15f;
    public static float offsetBeatTime = -0.07f;

    public static Action<float> OnBeat;
    public static Action<float> OnHalfBeat;
    public static Action<float> OnAfterWindowBeat;

    public static bool CalculateIfTimeIsInWindow(float time)
    {
        float inputTimeAccuracy = (time + offsetBeatTime) % secondsPerBeat;
        return inputTimeAccuracy < windowBeatTime / 2 || inputTimeAccuracy > secondsPerBeat - (windowBeatTime / 2);
    }
}
