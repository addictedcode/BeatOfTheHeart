using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private bool hadInputThisBeat = false;

    private void Awake()
    {
        BeatsManager.OnHalfBeat += OnHalfBeat;
    }

    private void OnDestroy()
    {
        BeatsManager.OnHalfBeat -= OnHalfBeat;
    }

    private void OnHalfBeat(float time)
    {
        hadInputThisBeat = false;
    }

    public void OnDodge(InputValue value)
    {
        float dir = value.Get<float>();
        if (dir != 0)
        {
            if (hadInputThisBeat)
                return; //Maybe add consequence
            if (BeatsManager.CalculateIfTimeIsInWindow(MusicManager.audioSource.time))
            {
                hadInputThisBeat = true;
                GameManager.Instance.TileManager.MoveToTile((int)dir);
            }
            else
            {
                //Punish
            }
        }
    }

    public void OnAttack()
    {
        if (hadInputThisBeat)
            return; //Maybe add consequence
        if (BeatsManager.CalculateIfTimeIsInWindow(MusicManager.audioSource.time))
        {
            hadInputThisBeat = true;
            //Attack
        }
        else
        {
            //Punish
        }
    }

    public void OnReflect()
    {
        if (hadInputThisBeat)
            return; //Maybe add consequence
        if (BeatsManager.CalculateIfTimeIsInWindow(MusicManager.audioSource.time))
        {
            hadInputThisBeat = true;
            //Reflect
        }
        else
        {
            //Punish
        }
    }
}
