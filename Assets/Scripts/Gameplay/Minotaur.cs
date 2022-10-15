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
            int rand = Random.Range(0, 3);
            switch (rand)
            {
                case 0:
                    MeleeAttack();
                    break;
                case 1:
                    ProjectileAttack();
                    break;
                case 2:
                    MultiAttack();
                    break;
            }

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
