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
        StartCoroutine(DisableGameObjectTimer());
    }

    private IEnumerator DisableGameObjectTimer()
    {
        ps.Play();
        yield return lifespan;
        gameObject.SetActive(false);
    }
}
