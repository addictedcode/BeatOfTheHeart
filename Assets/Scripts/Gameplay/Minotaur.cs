using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur : MonoBehaviour
{
    #region Enum States
    private enum MinotaurState
    {
        Idle,
        LeftWindup,
        RightWindup,
        LeftWindupProjectile,
        RightWindupProjectile,
        Attack,
    }

    [System.Serializable]
    public enum MinotaurAttacks
    {
        Idle,
        LSmash,
        RSmash,
        LFireball,
        RFireball,
        LRFireball
    }
    #endregion

    [Header("General")]
    private int maxHealth = 100;
    [SerializeField] private int health = 100;
    [SerializeField] private int meleeDamage = 1;
    [SerializeField] private int projectileDamage = 1;
    [SerializeField] private int specialDamage = 2;

    [Header("Combo")]
    [SerializeField] private MinotaurPhaseComboInfo[] minotaurPhaseComboInfoList;
    private int currentMinotaurPhaseComboInfoIndex = 0;

    private bool isCombat = false;
    public int Health => health;

    //Debugging States
    [SerializeField] private MinotaurState minotaurState = MinotaurState.Idle;
    private Animator animator;
    private Queue<MinotaurAttacks> currentComboSet = new();
    private Queue<MinotaurAttacks> queuedAttacks = new();
    private Queue<GameObject> queuedFireballs = new();

    [Header("Animations")]
    [SerializeField] private AnimationClip spawnAnim;
    [SerializeField] private AnimationClip deathAnim;

    [Header("VFX")] 
    [SerializeField] private Renderer renderer;
    [SerializeField] private ParticleSystem hitVFX;
    [SerializeField] private Material originalMat;
    [SerializeField] private Material hitMat;
    

    #region Unity Functions
    private void Awake()
    {
        maxHealth = health;
        animator = GetComponent<Animator>();
        BeatsManager.OnBeat += DoMove;
        renderer = GetComponent<Renderer>();
        originalMat = renderer.material;
    }

    private void OnDestroy()
    {
        BeatsManager.OnBeat -= DoMove;
    }

    public void StartMinotaur()
    {
        isCombat = false;
        animator.Play(spawnAnim.name);
        SFXManager.Instance.PlayOneShot("Spawn");
    }

    public void StartCombat()
    {
        isCombat = true;
    }


    #endregion

    #region Actions
    //Runs whenever On Beat is invoked
    private void DoMove(float num)
    {
        if (!isCombat) return;
        //If has nothing queued up, load a random combo
        if (currentComboSet.Count <= 0 && queuedAttacks.Count <= 0)
        {
            LoadRandomCombo();
        }

        //Change state based on queued up attacks, if empty, change to attack state
        if (currentComboSet.TryDequeue(out MinotaurAttacks attack))
        {
            ChangeStateBaseOnAttack(attack);
        }
        else if (queuedAttacks.Count > 0)
        {
            minotaurState = MinotaurState.Attack;
        }

        switch (minotaurState)
        {
            case MinotaurState.Idle:
                Idle();
                break;
            case MinotaurState.LeftWindup:
                WindupMelee(1);
                break;
            case MinotaurState.RightWindup:
                WindupMelee(0);
                break;
            case MinotaurState.LeftWindupProjectile:
                WindupProjectile(1);
                break;
            case MinotaurState.RightWindupProjectile:
                WindupProjectile(0);
                break;
            case MinotaurState.Attack:
                DecideAttack();
                break;
            default:
                break;
        }
    }

    private void ChangeStateBaseOnAttack(MinotaurAttacks move)
    {
        switch (move)
        {
            case MinotaurAttacks.Idle:
                minotaurState = MinotaurState.Idle;
                break;
            case MinotaurAttacks.LSmash:
                minotaurState = MinotaurState.LeftWindup;
                break;
            case MinotaurAttacks.RSmash:
                minotaurState = MinotaurState.RightWindup;
                break;
            case MinotaurAttacks.LFireball:
                minotaurState = MinotaurState.LeftWindupProjectile;
                break;
            case MinotaurAttacks.RFireball:
                minotaurState = MinotaurState.RightWindupProjectile;
                break;
            default:
                minotaurState = MinotaurState.Idle;
                break;

        }
    }
    private void Idle()
    {
        animator.Play("Idle");
        queuedAttacks.Enqueue(MinotaurAttacks.Idle);
    }

    private void DecideAttack()
    {
        MinotaurAttacks attack = queuedAttacks.Dequeue();
        switch (attack)
        {
            case MinotaurAttacks.LSmash:
                StartCoroutine(DoMeleeAttack(meleeDamage, 1, "LeftSwing", "Slam"));
                break;
            case MinotaurAttacks.RSmash:
                StartCoroutine(DoMeleeAttack(meleeDamage, 0, "RightSwing", "Slam"));
                break;
            case MinotaurAttacks.LFireball:
                StartCoroutine(DoProjectileAttack(projectileDamage, 1, "Idle", "Fireball"));
                break;
            case MinotaurAttacks.RFireball:
                StartCoroutine(DoProjectileAttack(projectileDamage, 0, "Idle", "Fireball"));
                break;
            case MinotaurAttacks.Idle:
                animator.Play("Idle");
                break;
            default:
                break;
        }
    }

    private IEnumerator DoMeleeAttack(int damage, int tile, string anim, string sfx)
    {
        animator.Play(anim);
        SFXManager.Instance.PlayOneShot(sfx);
        GameManager.Instance.TriggerIndicator(tile);
        GameManager.Instance.SpawnGroundSmash(tile);
        yield return new WaitForSeconds(0.18f);
        GameManager.Instance.CheckPlayerTakeDamage(damage, tile);
    }

    private IEnumerator DoProjectileAttack(int damage, int tile, string anim, string sfx)
    {
        animator.Play(anim);
        SFXManager.Instance.PlayOneShot(sfx);
        GameManager.Instance.ShootFireball(tile, queuedFireballs.Dequeue());
        GameManager.Instance.SpawnExplosion(tile);
        yield return new WaitForSeconds(0.18f);
        if (!GameManager.Instance.Player.isReflect)
            GameManager.Instance.CheckPlayerTakeDamage(damage, tile);
        else
        {
            GameManager.Instance.ActivateReflectFireball(tile);
            TakeDamage(GameManager.Instance.Player.GetReflectDamage());
        }
    }

    private void WindupMelee(int tile)
    {
        if (tile == 0)
        {
            animator.Play("RightWindup");
            queuedAttacks.Enqueue(MinotaurAttacks.RSmash);
        }
        else
        {
            animator.Play("LeftWindup");
            queuedAttacks.Enqueue(MinotaurAttacks.LSmash);
        }
        GameManager.Instance.SpawnIndicator(tile);
    }

    private void WindupProjectile(int tile)
    {
        animator.Play("WindupProjectile");
        queuedAttacks.Enqueue(tile == 0 ? MinotaurAttacks.RFireball : MinotaurAttacks.LFireball);
        queuedFireballs.Enqueue(GameManager.Instance.SpawnFireball(tile));
    }
    #endregion

    #region Health
    public void TakeDamage(int damage)
    {
        health -= damage;
        StartCoroutine(Flash());
        UpdateMinotaurPhase();
        if (health <= 0) Death();
    }

    private void Death()
    {
        GameManager.Instance.EndCombat(true);
        animator.Play(deathAnim.name);
        Destroy(this);
    }

    public void PlayerDeath()
    {
        animator.Play("Idle");
        Destroy(this);
    }
    #endregion

    #region Combo
    private void LoadCombo(int set) => minotaurPhaseComboInfoList[currentMinotaurPhaseComboInfoIndex].comboList.comboList[set].combo.ForEach(o => currentComboSet.Enqueue(o));
    private void LoadRandomCombo() => LoadCombo(Random.Range(0, minotaurPhaseComboInfoList[currentMinotaurPhaseComboInfoIndex].comboList.comboList.Count));

    private void UpdateMinotaurPhase()
    {
        //Check if last phase
        if (currentMinotaurPhaseComboInfoIndex + 1 >= minotaurPhaseComboInfoList.Length) return;

        float HPThreshold = minotaurPhaseComboInfoList[currentMinotaurPhaseComboInfoIndex + 1].HPThreshold;
        if (health <= (float)maxHealth * HPThreshold)
        {
            currentMinotaurPhaseComboInfoIndex++;
        }
    }
    #endregion


    #region VFX

    IEnumerator Flash()
    {
        renderer.material = hitMat;
        yield return new WaitForSeconds(.1f);
        renderer.material = originalMat;
        
    }

    #endregion
}
