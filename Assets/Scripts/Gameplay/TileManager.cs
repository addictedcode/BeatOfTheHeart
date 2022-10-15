using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Transform[] tiles;

    public int currentTile = 1;

    public void MoveToTile(int dir)
    {
        if (CheckValidTile(dir))
            GameManager.Instance.Player.transform.position = tiles[currentTile += dir].transform.position;
        else
            Debug.Log("Failed Jump");
    }

    private bool CheckValidTile(int dir) => currentTile + dir >= 0 && currentTile + dir < tiles.Length;
}
