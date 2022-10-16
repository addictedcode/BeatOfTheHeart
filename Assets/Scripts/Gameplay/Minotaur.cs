using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur : MonoBehaviour
{
    private enum MinotaurState
    {
        Idle,
        LeftWindup,
        RightWindup,
        Attack,
        Cooldown
    }

    [SerializeField] private int health = 100;
    [SerializeField] private MinotaurComboSO[] minotaurComboSOs;

    private int currentCombo;

    //Debugging States
    [SerializeField] private MinotaurState minotaurState = MinotaurState.Idle;
    private Animator animator;
    private Queue<string> currentComboSet = new();

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
            case MinotaurState.LeftWindup:
            case MinotaurState.RightWindup:
                DoAttack();
                break;
            case MinotaurState.Attack:
                DecideAttack();
                break;
            case MinotaurState.Cooldown:
                break;
            default:
                break;
        }
    }

    private void DecideAttack()
    {
        if (currentComboSet.TryDequeue(out string combo))
        {
            switch (combo)
            {
                case "MeleeLeft":
                    MeleeLeft();
                    break;
                case "MeleeRight":
                    MeleeRight();
                    break;
                default:
                    MeleeLeft();
                    break;
            }
        }
        else
        {
            if (Random.Range(0, 2) == 0)
                MeleeLeft();
            else
                MeleeRight();
        }
        
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
    }

    private void DoAttack()
    {
        switch (minotaurState)
        {
            case MinotaurState.LeftWindup:
                animator.Play("LeftSwing");
                SFXManager.Instance.PlayOneShot("Slam");
                break;
            case MinotaurState.RightWindup:
                animator.Play("RightSwing");
                SFXManager.Instance.PlayOneShot("Slam");
                break;
        }

        minotaurState = MinotaurState.Attack;
    }

    private void MeleeLeft()
    {
        animator.Play("RightWindup");
        GameManager.Instance.ActivateIndicator(0);
        minotaurState = MinotaurState.RightWindup;
    }

    private void MeleeRight()
    {
        animator.Play("LeftWindup");
        GameManager.Instance.ActivateIndicator(1);
        minotaurState = MinotaurState.LeftWindup;
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

    #region Combo
    private void LoadCombo(int set)
    {
        foreach (string combo in minotaurComboSOs[set].combo)
            currentComboSet.Enqueue(combo);
    }
    #endregion
}
