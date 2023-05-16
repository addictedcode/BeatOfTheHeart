using System.Collections;
using UnityEngine;

public class minoAttackIndicator : MonoBehaviour
{
    private readonly WaitForSeconds lifespan = new(0.55f);
    Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void OnTrigger()
    {
        StartCoroutine(DestroyGameObjectTimer());
        animator.enabled = true;
        animator.Play("targetAnim");
    }

    private IEnumerator DestroyGameObjectTimer()
    {
        yield return lifespan;
        Destroy(gameObject);
    }
}