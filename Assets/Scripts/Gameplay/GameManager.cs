using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TileManager tileManager;
    [SerializeField] private Forte player;
    [SerializeField] private Minotaur minotaur;

    public static bool IsPlaying = true;

    public Forte Player => player;
    public Minotaur Minotaur => minotaur;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void EndGame(bool isVictory)
    {
        IsPlaying = false;
    }
}
