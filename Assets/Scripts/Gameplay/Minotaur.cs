using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur : MonoBehaviour
{
    private enum MinotaurState
    {
        Idle,
        Windup,
        Attack,
        Cooldown
    }

    private enum WindupState
    {
        None,
        LeftWindup,
        RightWindup,
    }

    [SerializeField] private int health = 100;

    private int currentCombo;

    [SerializeField] private MinotaurState minotaurState = MinotaurState.Idle;
    [SerializeField] private WindupState windupState = WindupState.None;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        BeatsManager.OnBeat += DoMove;
    }

    private void OnDestroy()
    {
        BeatsManager.OnBeat -= DoMove;
    }

    #region Actions
    private void DoMove(float num)
    {
        switch (minotaurState)
        {
            case MinotaurState.Idle:
                DecideAttack();
                break;
            case MinotaurState.Windup:
                DoAttack();
                break;
            case MinotaurState.Attack:
                DecideAttack();
                break;
            case MinotaurState.Cooldown:
                break;
        }
        
    }

    private void DecideAttack()
    {
        MeleeAttack();
        //if (currentCombo < 4)
        //{
        //    int rand = Random.Range(0, 3);
        //    switch (rand)
        //    {
        //        case 0:
        //            MeleeAttack();
        //            break;
        //        case 1:
        //            ProjectileAttack();
        //            break;
        //        case 2:
        //            MultiAttack();
        //            break;
        //    }

        //    currentCombo++;
        //}
        //else
        //{
        //    SpecialAttack();

        //    currentCombo = 0;
        //}

        minotaurState = MinotaurState.Windup;
    }

    private void DoAttack()
    {
        switch (windupState)
        {
            case WindupState.LeftWindup:
                animator.Play("LeftSwing");
                break;
            case WindupState.RightWindup:
                animator.Play("RightSwing");
                break;
        }

        minotaurState = MinotaurState.Attack;
        windupState = WindupState.None;
    }

    private void MeleeAttack()
    {
        if (Random.Range(0, 2) == 0)
        {
            GameManager.Instance.spawnIndicator(1);
            animator.Play("LeftWindup");
            windupState = WindupState.LeftWindup;
        }
        else
        {
            GameManager.Instance.spawnIndicator(0);
            animator.Play("RightWindup");
            windupState = WindupState.RightWindup;
        }
    }

    private void ProjectileAttack()
    {

    }

    private void MultiAttack()
    {

    }

    private void SpecialAttack()
    {

    }
    #endregion

    #region Health
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Death();
    }

    private void Death()
    {
        GameManager.Instance.EndGame(false);
    }
    #endregion
}
