using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private readonly WaitForSeconds lifespan = new(0.5f);
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponentInChildren<ParticleSystem>();
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyGameObjectTimer());
    }

    private IEnumerator DestroyGameObjectTimer()
    {
        ps.Play();
        yield return lifespan;
        Destroy(gameObject);
    }
}
