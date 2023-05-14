using System.Collections;
using UnityEngine;

public class Forte : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private int health = 4;

    public int Health => health;
    [SerializeField] private int attackDamage = 2;
    [SerializeField] private int reflectDamage = 5;

    [Header("Super")]
    [SerializeField] private int attackAddSuper = 3;
    [SerializeField] private int reflectAddSuper = 10;
    [SerializeField] private int superMeterValue = 0;

    private readonly int maxSuperMeter = 100;
    public PlayerActions lastAction;
    public bool isReflect;

    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        BeatsManager.OnBeat -= backToIdle;
    }

    public enum PlayerActions
    {
        Idle,
        MoveToLeft,
        MoveToRight,
        Attack,
        Reflect
    }

    private void backToIdle(float num)
    {
        animator.Play("PC_Idle");
    }

    #region Actions
    private IEnumerator turnOffReflectCoroutine()
    {
        yield return new WaitForSeconds(.35f);
        isReflect = false;
    }

    public void Move(int dir)
    {
        animator.Play("PC_Fear");
        GameManager.Instance.TileManager.MoveToTile(dir);
        SFXManager.Instance.PlayOneShot("Jump");
    }

    public void Attack()
    {
        animator.Play("PC_Anger");
        int totalDamage = Mathf.RoundToInt(attackDamage * GameManager.Instance.GetPlayerComboDamageMultiplier());
        GameManager.Instance.Minotaur.TakeDamage(totalDamage);
        AddSuperMeterValue(attackAddSuper);
        SFXManager.Instance.PlayOneShot("Attack");
    }

    public void Reflect()
    {
        animator.Play("PC_Happy");
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
        animator.Play("PC_Death");
        GameManager.Instance.EndGame(false);
    }

    //void Update()
    //{
    //    Debug.Log(lastAction);
    //}
    #endregion

    public int GetReflectDamage()
    {
        int totalDamage = Mathf.RoundToInt(reflectDamage * GameManager.Instance.GetPlayerComboDamageMultiplier());
        return totalDamage;
}
    private void AddSuperMeterValue(int value) => superMeterValue = Mathf.Clamp(superMeterValue += value, 0, maxSuperMeter);
}
