using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public GameObject beatIndication;

    private void Awake()
    {
        BeatsManager.OnBeat += OnBeat;
    }

    private void OnDestroy()
    {
        BeatsManager.OnBeat -= OnBeat;
    }

    private void OnBeat(float time)
    {
    }

    public void OnDodge(InputValue value)
    {
        float dir = value.Get<float>();
        if (dir < 0)
        {
            Debug.Log("Dodge Left");
        }
        else if (dir > 0)
        {
             Debug.Log("Dodge Right");
        }
    }

    public void OnAttack()
    {
        Debug.Log(BeatsManager.CalculateIfTimeIsInWindow(MusicManager.audioSource.time));
    }

    public void OnReflect()
    {
        Debug.Log("Reflecting");
    }
}
