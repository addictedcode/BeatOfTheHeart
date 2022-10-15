using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minoAttackIndicator : MonoBehaviour
{

    private float lifespan = .5f;
    private float lifetime = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        lifetime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        lifetime += Time.deltaTime;
        if (lifetime >= lifespan)
        {
            Destroy(this.gameObject);
        }
    }
}
