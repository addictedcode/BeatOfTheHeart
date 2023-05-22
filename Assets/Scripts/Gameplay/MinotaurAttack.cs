using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurAttack : MonoBehaviour
{
    protected readonly WaitForSeconds lifespan = new WaitForSeconds(BeatsManager.secondsPerBeat);
    protected Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnShoot()
    {
        StartCoroutine(DestroyGameObjectTimer());
        animator.enabled = true;
    }

    private IEnumerator DestroyGameObjectTimer()
    {
        yield return lifespan;
        Destroy(gameObject);
    }
}
