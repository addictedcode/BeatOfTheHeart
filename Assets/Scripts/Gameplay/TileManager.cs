using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Transform[] tiles;
    [SerializeField] private GameObject attackIndicatorPrefab;

    [Header("Fireball FX")]
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject reflectFireballPrefab;

    public int currentTile = 1;

    private GameObject[] attackIndicators;
    private GameObject[] fireballs;
    private GameObject[] explosions;
    private GameObject[] reflectFireball;

    private void Awake()
    {
        attackIndicators = new GameObject[tiles.Length];
        for(int i = 0; i < tiles.Length; i++)
            attackIndicators[i] = Instantiate(attackIndicatorPrefab, tiles[i].transform);

        fireballs = new GameObject[tiles.Length];
        for (int i = 0; i < tiles.Length; i++)
            fireballs[i] = Instantiate(fireballPrefab, tiles[i].transform);

        explosions = new GameObject[tiles.Length];
        for (int i = 0; i < tiles.Length; i++)
            explosions[i] = Instantiate(explosionPrefab, tiles[i].transform);

        reflectFireball = new GameObject[tiles.Length];
        for (int i = 0; i < tiles.Length; i++)
            reflectFireball[i] = Instantiate(reflectFireballPrefab, tiles[i].transform);
    }

    public void MoveToTile(int dir)
    {
        if (CheckValidSideTile(dir))
            GameManager.Instance.Player.transform.position = tiles[currentTile += dir].transform.position;
        else
            Debug.Log("Failed Jump");
    }

    public bool CheckValidSideTile(int dir) => currentTile + dir >= 0 && currentTile + dir < tiles.Length;
    public bool CheckValidTile(int tile) => tile >= 0 && tile < tiles.Length;
    public void ActivateIndicator(int num)
    {
        if (CheckValidTile(num)) attackIndicators[num].SetActive(true);
    }

    public void ActivateFireball(int num)
    {
        if (CheckValidTile(num)) fireballs[num].SetActive(true);
    }

    public void ActivateExplosion(int num)
    {
        if (CheckValidTile(num)) explosions[num].SetActive(true);
    }

    public void ActivateReflectFireball(int num)
    {
        if (CheckValidTile(num)) reflectFireball[num].SetActive(true);
    }
}
