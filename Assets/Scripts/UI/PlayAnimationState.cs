using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationState : MonoBehaviour
{
    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnimationByTrigger(string trigger)
    {
        animator.SetTrigger(trigger);
    }
}
