using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeatsListener : MonoBehaviour
{
    [SerializeField] private UnityEvent OnBeat;

    private void Awake()
    {
        BeatsManager.OnBeat += OnBeatManagerOnBeat;
    }

    private void OnDestroy()
    {
        BeatsManager.OnBeat -= OnBeatManagerOnBeat;
    }

    public void OnBeatManagerOnBeat(float time)
    {
        OnBeat?.Invoke();
    }
}
