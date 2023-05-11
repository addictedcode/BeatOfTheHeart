using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Phase
{
    public Minotaur minotaur;
    public TileManager tileManager;
    public Camera camera;
}
public enum PhaseState
{
    Spawning,
    Combat,
    CombatEnd,
    Transition
}

public class PhasesManager : MonoBehaviour
{
    public static PhasesManager Instance { get; private set; }

    [SerializeField] private List<Phase> phases = new List<Phase>();
    public List<Phase> Phases => phases;
    public int currentPhase = 0;
    public PhaseState currentPhaseState = PhaseState.Spawning;


    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void InitPhase(int newPhase)
    {
        Phases[currentPhase].minotaur.gameObject.SetActive(false);
        Phases[currentPhase].tileManager.gameObject.SetActive(false);
        Phases[currentPhase].camera.gameObject.SetActive(false);

        if (newPhase >= phases.Count)
        {
            currentPhase = 0;
        }
        else
        {
            currentPhase = newPhase;
        }

        GameManager.Instance.UpdatePhase(phases[currentPhase].minotaur, 
                                            phases[currentPhase].tileManager);
        Phases[currentPhase].minotaur.gameObject.SetActive(true);
        Phases[currentPhase].tileManager.gameObject.SetActive(true);
        Phases[currentPhase].camera.gameObject.SetActive(true);


        Phases[currentPhase].minotaur.StartMinotaur();
        Phases[currentPhase].tileManager.MoveToTile(0); 
    }


}
