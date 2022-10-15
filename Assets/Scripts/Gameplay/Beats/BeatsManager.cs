using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatsManager : MonoBehaviour
{
    [SerializeField] private uint BPM = 120;

    public Action OnBeat;

    void OnStartBeat()
    {

    }
}
