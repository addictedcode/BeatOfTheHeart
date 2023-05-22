using System.Collections;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private readonly WaitForSeconds lifespan = new(0.75f);
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnShoot()
    {
        StartCoroutine(DestroyGameObjectTimer());
        animator.enabled = true;
        animator.Play("FireballAnimation");
    }

    private IEnumerator DestroyGameObjectTimer()
    {
        yield return lifespan;
        Destroy(gameObject);
    }
}
