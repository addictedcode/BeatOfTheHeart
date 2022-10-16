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
    public TileManager TileManager => tileManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #region Minotaur
    public void CheckPlayerTakeDamage(int damage, int tile)
    {
        if (tileManager.currentTile == tile)
            player.TakeDamage(damage);
    }

    public void ActivateIndicator(int num) => tileManager.ActivateIndicator(num);
    #endregion

    public void EndGame(bool isVictory)
    {
        IsPlaying = false;
    }
}
