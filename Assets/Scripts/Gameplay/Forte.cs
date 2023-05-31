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
        int totalDamage = Mathf.RoundToInt(attackDamage * GameManager.Instance.GetPlayerComboDamageMultiplier());
        GameManager.Instance.Minotaur.TakeDamage(totalDamage);
        AddSuperMeterValue(attackAddSuper);
        AttackAnimation();
    }

    public void AttackAnimation()
    {
        animator.Play("PC_Anger");
        SFXManager.Instance.PlayOneShot("Attack");
    }

    public void Reflect()
    {
        ReflectAnimation();
        isReflect = true;
        AddSuperMeterValue(reflectAddSuper);
    }

    public void ReflectAnimation()
    {
        animator.Play("PC_Happy");
        SFXManager.Instance.PlayOneShot("Reflect");
        StartCoroutine(turnOffReflectCoroutine());
    }

    public void Stutter()
    {

    }

    public void PlayAnimation(string anim)
    {
        animator.Play(anim);
    }

    public void flipSprite(bool isFlipped)
    {
        GetComponent<SpriteRenderer>().flipX = isFlipped;
    }
    #endregion

    #region Health
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Death();

        SFXManager.Instance.PlayOneShot("Player_Hurt");
    }

    private void Death()
    {
        animator.Play("PC_Death");
        SFXManager.Instance.PlayOneShot("Death_Riff");
        GameManager.Instance.EndCombat(false);
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
