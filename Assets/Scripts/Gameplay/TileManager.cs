using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Transform[] tiles;
    [SerializeField] private GameObject attackIndicatorPrefab;
    
    public int currentTile = 1;

    [SerializeField]private GameObject[] attackIndicators;

    private void Awake()
    {
        attackIndicators = new GameObject[tiles.Length];
        for(int i = 0; i < tiles.Length; i++)
            attackIndicators[i] = Instantiate(attackIndicatorPrefab, tiles[i].transform);
    }

    public void MoveToTile(int dir)
    {
        if (CheckValidSideTile(dir))
            GameManager.Instance.Player.transform.position = tiles[currentTile += dir].transform.position;
        else
            Debug.Log("Failed Jump");
    }

    private bool CheckValidSideTile(int dir) => currentTile + dir >= 0 && currentTile + dir < tiles.Length;
    private bool CheckValidTile(int tile) => tile >= 0 && tile < tiles.Length;
    public void ActivateIndicator(int num)
    {
        if (CheckValidTile(num)) attackIndicators[num].SetActive(true);
    }
}
