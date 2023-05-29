using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private Forte player;
    [SerializeField] private GateMinigame gateMinigame;
    private bool hadInputThisBeat = false;

    private void Awake()
    {
        player = GetComponent<Forte>();
        BeatsManager.OnHalfBeat += OnHalfBeat;
    }

    private void OnDestroy()
    {
        BeatsManager.OnHalfBeat -= OnHalfBeat;
    }

    private void OnHalfBeat(float time)
    {
        if (hadInputThisBeat)
            hadInputThisBeat = false;
        else
            GameManager.Instance.ResetComboMeter();
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
                GameManager.Instance.IncrementComboCount();
                hadInputThisBeat = true;
                player.Move((int)dir);
            }
            else
            {
                //Punish
                PunishMissInput();
            }
        }
    }

    public void OnAttack()
    {
        if (hadInputThisBeat)
            return; //Maybe add consequence
        if (BeatsManager.CalculateIfTimeIsInWindow(MusicManager.audioSource.time))
        {
            GameManager.Instance.IncrementComboCount();
            
            hadInputThisBeat = true;
            player.Attack();
        }
        else
        {
            //Punish
            PunishMissInput();
        }
    }

    public void OnReflect()
    {
        if (hadInputThisBeat)
            return; //Maybe add consequence
        if (BeatsManager.CalculateIfTimeIsInWindow(MusicManager.audioSource.time))
        {
            GameManager.Instance.IncrementComboCount();
            hadInputThisBeat = true;
            player.Reflect();
        }
        else
        {
            //Punish
            PunishMissInput();
        }
    }

    public void OnDirection(InputValue value)
    {
        if (hadInputThisBeat)
            return; //Maybe add consequence

        Vector2 direction = value.Get<Vector2>();
        if (BeatsManager.CalculateIfTimeIsInWindow(MusicManager.audioSource.time))
        {
            gateMinigame.OnDirection(direction);
        }
        else
        {
            gateMinigame.OnFailedInput();
        }
        hadInputThisBeat = true;
    }

    private void PunishMissInput()
    {
        GameManager.Instance.ResetComboMeter();
        SFXManager.Instance.PlayOneShot("Miss");
        hadInputThisBeat = true;
    }
}
