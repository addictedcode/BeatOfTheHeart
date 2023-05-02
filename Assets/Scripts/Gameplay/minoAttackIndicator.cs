using System.Collections;
using UnityEngine;

public class minoAttackIndicator : MonoBehaviour
{
    private readonly WaitForSeconds lifespan = new(0.55f);

    private void OnEnable()
    {
        StartCoroutine(DisableGameObjectTimer());
        GetComponentInChildren<Animator>().enabled = true;
    }

    private IEnumerator DisableGameObjectTimer()
    {
        yield return lifespan;
        gameObject.SetActive(false);
        GetComponentInChildren<Animator>().enabled = false;
    }
}