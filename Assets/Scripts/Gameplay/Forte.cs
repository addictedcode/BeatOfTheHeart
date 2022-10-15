using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forte : MonoBehaviour
{
    [SerializeField] private int health = 3;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private int reflectDamage = 5;

    #region Actions
    

    public void Attack()
    {

    }

    public void Reflect()
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
