using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur : MonoBehaviour
{
    [SerializeField] private int health = 100;

    private int currentCombo;
    private bool canAttack;

    private void Awake()
    {
        BeatsManager.OnBeat += DecideMove;
    }

    private void OnDestroy()
    {
        BeatsManager.OnBeat -= DecideMove;
    }

    #region Actions
    private void DecideMove(float num)
    {
        if (!canAttack) return;

        if(currentCombo < 4)
        {
            if (Random.Range(0, 2) == 0)
                MeleeAttack();
            else
                ProjectileAttack();

            currentCombo++;
        }
        else
        {
            SpecialAttack();

            currentCombo = 0;
        }

        canAttack = false;
    }

    private void MeleeAttack()
    {

    }

    private void ProjectileAttack()
    {

    }

    private void SpecialAttack()
    {

    }

    private void MultiAttack()
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

    }
    #endregion
}
