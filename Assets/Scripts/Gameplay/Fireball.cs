using System.Collections;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private readonly WaitForSeconds lifespan = new(0.5f);
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StartCoroutine(DisableGameObjectTimer());
        animator.enabled = true;
    }

    private IEnumerator DisableGameObjectTimer()
    {
        yield return lifespan;
        gameObject.SetActive(false);
        animator.enabled = false;
    }
}
