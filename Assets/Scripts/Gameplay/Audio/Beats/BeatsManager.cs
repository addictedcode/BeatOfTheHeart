using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BeatsManager
{
    public static uint BPM = 120;

    public static Action<float> OnBeat;
    public static Action OnBeforeBeatWindow;
    public static Action OnAfterBeatWindow;

}
