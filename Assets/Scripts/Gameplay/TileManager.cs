using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance { get; private set; }

    [SerializeField] private GameObject player;
    [SerializeField] private Transform[] tiles;

    private int currentTile = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void MoveToTile(int dir)
    {
        if (CheckValidTile(dir))
        {
            player.transform.position = tiles[currentTile += dir].transform.position;
        }
            
        else
            Debug.Log("Failed Jump");
    }

    private bool CheckValidTile(int dir) => currentTile + dir >= 0 && currentTile + dir < tiles.Length;
}
