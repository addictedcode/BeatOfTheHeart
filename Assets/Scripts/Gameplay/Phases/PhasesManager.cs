using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Phase
{
    public Minotaur minotaur;
    public TileManager tileManager;
}

public class PhasesManager : MonoBehaviour
{

    [SerializeField] private List<Phase> phases = new List<Phase>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
