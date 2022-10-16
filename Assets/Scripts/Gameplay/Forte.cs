using System.Collections;
using UnityEngine;

public class Forte : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private int health = 4;

    public int Health => health;
    [SerializeField] private int attackDamage = 5;
    [SerializeField] private int reflectDamage = 2;

    [Header("Super")]
    [SerializeField] private int attackAddSuper = 3;
    [SerializeField] private int reflectAddSuper = 10;
    [SerializeField] private int superMeterValue = 0;

    private readonly int maxSuperMeter = 100;
    public PlayerActions lastAction;
    public bool isReflect;

    public enum PlayerActions
    {
        Idle,
        MoveToLeft,
        MoveToRight,
        Attack,
        Reflect
    }

    #region Actions
    private IEnumerator turnOffReflectCoroutine()
    {
        yield return new WaitForSeconds(.35f);
        isReflect = false;
    }

    public void Move(int dir)
    {
        GameManager.Instance.TileManager.MoveToTile(dir);
        SFXManager.Instance.PlayOneShot("Jump");
    }

    public void Attack()
    {
        GameManager.Instance.Minotaur.TakeDamage(attackDamage);
        AddSuperMeterValue(attackAddSuper);
        SFXManager.Instance.PlayOneShot("Attack");
    }

    public void Reflect()
    {
        isReflect = true;
        AddSuperMeterValue(reflectAddSuper);
        SFXManager.Instance.PlayOneShot("Reflect");
        StartCoroutine(turnOffReflectCoroutine());
    }

    public void Stutter()
    {

    }
    #endregion

    #region Health
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Death();

        SFXManager.Instance.PlayOneShot("Hurt");
    }

    private void Death()
    {
        GameManager.Instance.EndGame(false);
    }

    //void Update()
    //{
    //    Debug.Log(lastAction);
    //}
    #endregion

    private void AddSuperMeterValue(int value) => superMeterValue = Mathf.Clamp(superMeterValue += value, 0, maxSuperMeter);
}
