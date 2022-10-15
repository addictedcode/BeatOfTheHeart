using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private bool isInBeatWindow = false;
    private float onBeatTime;

    private void Awake()
    {
        BeatsManager.OnBeforeBeatWindow += OnBeforeBeatWindow;
        BeatsManager.OnAfterBeatWindow += OnAfterBeatWindow;
        BeatsManager.OnBeat += OnBeat;
    }

    private void OnDestroy()
    {
        BeatsManager.OnBeforeBeatWindow -= OnBeforeBeatWindow;
        BeatsManager.OnAfterBeatWindow -= OnAfterBeatWindow;
        BeatsManager.OnBeat -= OnBeat;
    }

    private void OnBeat(float time)
    {
        onBeatTime = time;
    }

    private void OnBeforeBeatWindow() 
    {
        isInBeatWindow = true;
    }

    private void OnAfterBeatWindow()
    {
        isInBeatWindow = false;
    }

    public void OnDodge(InputValue value)
    {
        float dir = value.Get<float>();
        if (dir < 0)
        {
            if (isInBeatWindow)
            {
                Debug.Log("Dodge Left");
            }
            else
            {
                Debug.Log("Fail Dodge Left");
            }
        }
        else if (dir > 0)
        {
            if (isInBeatWindow)
            {
                Debug.Log("Dodge Right");
            }
            else
            {
                Debug.Log("Fail Dodge Right");
            }
        }
    }

    public void OnAttack()
    {
        Debug.Log(Time.time - onBeatTime);
        /*if (isInBeatWindow)
        {
            Debug.Log("Attacking");
        }
        else
        {
            Debug.Log("Fail Attack");
        }*/
    }

    public void OnReflect()
    {
        if (isInBeatWindow)
        {
            Debug.Log("Reflecting");
        }
        else
        {
            Debug.Log("Fail Reflect");
        }
    }
}
