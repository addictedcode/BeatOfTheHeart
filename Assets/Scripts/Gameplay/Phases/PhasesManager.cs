using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Phase
{
    public Minotaur minotaur;
    public TileManager tileManager;
    public Forte player;
    public Camera camera;
}

public class PhasesManager : MonoBehaviour
{
    public static PhasesManager Instance { get; private set; }

    [SerializeField] private List<Phase> phases = new List<Phase>();
    public List<Phase> Phases => phases;
    public int currentPhase = 0;


    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void MoveToNextPhase()
    {
    }

    public void InitPhase(int newPhase)
    {
        phases[currentPhase].minotaur.gameObject.SetActive(false);
        //phases[currentPhase].player.gameObject.SetActive(false);
        phases[currentPhase].tileManager.gameObject.SetActive(false);
        phases[currentPhase].camera.gameObject.SetActive(false);

        if (newPhase >= phases.Count)
        {
            currentPhase = 0;
        }
        else
        {
            currentPhase = newPhase;
        }

        GameManager.Instance.UpdatePhase(phases[currentPhase].minotaur, 
                                            phases[currentPhase].player, 
                                            phases[currentPhase].tileManager);
        phases[currentPhase].minotaur.gameObject.SetActive(true);
        //phases[currentPhase].player.gameObject.SetActive(true);
        phases[currentPhase].tileManager.gameObject.SetActive(true);
        phases[currentPhase].camera.gameObject.SetActive(true);


        phases[currentPhase].tileManager.MoveToTile(0);
    }


}
