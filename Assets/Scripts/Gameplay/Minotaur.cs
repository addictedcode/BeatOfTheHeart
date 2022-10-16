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
        LeftWindupProjectile,
        RightWindupProjectile,
        Attack,
        Cooldown
    }

    [Header("General")]
    [SerializeField] private int health = 100;
    [SerializeField] private int meleeDamage = 1;
    [SerializeField] private int projectileDamage = 1;
    [SerializeField] private int specialDamage = 2;

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
        animator.Play("Idle");
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
            case MinotaurState.LeftWindupProjectile:
            case MinotaurState.RightWindupProjectile:
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
                    WindupMelee(0);
                    break;
                case "MeleeRight":
                    WindupMelee(1);
                    break;
                case "ProjectileLeft":
                    WindupProjectile(0);
                    break;
                case "ProjectileRight":
                    WindupProjectile(1);
                    break;
                default:
                    WindupMelee(0);
                    break;
            }
        }
        else
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    WindupMelee(0);
                    break;
                case 1:
                    WindupMelee(1);
                    break;
                case 2:
                    WindupProjectile(0);
                    break;
                case 3:
                    WindupProjectile(1);
                    break;
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
    }

    private void DoAttack()
    {
        switch (minotaurState)
        {
            case MinotaurState.LeftWindup:
                StartCoroutine(DoAttack(meleeDamage, 1, "LeftSwing", "Slam"));
                break;
            case MinotaurState.RightWindup:
                StartCoroutine(DoAttack(meleeDamage, 0, "RightSwing", "Slam"));
                break;
            case MinotaurState.LeftWindupProjectile:
                StartCoroutine(DoAttack(projectileDamage, 1, "Idle", "Fireball"));
                GameManager.Instance.ActivateExplosion(1);
                break;
            case MinotaurState.RightWindupProjectile:
                StartCoroutine(DoAttack(projectileDamage, 0, "Idle", "Fireball"));
                GameManager.Instance.ActivateExplosion(0);
                break;
            default:
                break;
        }

        minotaurState = MinotaurState.Attack;
    }

    private IEnumerator DoAttack(int damage, int tile, string anim, string sfx)
    {
        animator.Play(anim);
        SFXManager.Instance.PlayOneShot(sfx);
        yield return new WaitForSeconds(0.18f);
        GameManager.Instance.CheckPlayerTakeDamage(damage, tile);
    }

    private void WindupMelee(int tile)
    {
        if (tile == 0)
        {
            animator.Play("RightWindup");
            minotaurState = MinotaurState.RightWindup;
        }
        else
        {
            animator.Play("LeftWindup");
            minotaurState = MinotaurState.LeftWindup;
        }
        GameManager.Instance.ActivateIndicator(tile);
    }

    private void WindupProjectile(int tile)
    {
        animator.Play("WindupProjectile");
        GameManager.Instance.ActivateIndicator(tile);
        GameManager.Instance.ActivateFireball(tile);
        minotaurState = tile == 0 ? MinotaurState.RightWindupProjectile : MinotaurState.LeftWindupProjectile;
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
    private void LoadCombo(int set) => minotaurComboSOs[set].combo.ForEach(o => currentComboSet.Enqueue(o));
    private void LoadRandomCombo() => LoadCombo(Random.Range(0, minotaurComboSOs.Length));
    #endregion
}
