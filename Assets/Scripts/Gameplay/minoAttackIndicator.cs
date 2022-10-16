using System.Collections;
using UnityEngine;

public class MinoAttackIndicator : MonoBehaviour
{
    private readonly WaitForSeconds lifespan = new(0.5f);

    private void OnEnable()
    {
        StartCoroutine(DisableGameObjectTimer());
    }

    private IEnumerator DisableGameObjectTimer()
    {
        yield return lifespan;
        gameObject.SetActive(false);
    }
}