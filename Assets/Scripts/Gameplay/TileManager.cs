using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Transform[] tiles;
    [SerializeField] private ScalingSpawner[] attackIndicatorSpawners;
    [SerializeField] private GameObject attackIndicatorPrefab;

    [Header("Fireball FX")]
    [SerializeField] private CircularSpawner[] fireballSpawners;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject reflectFireballPrefab;

    [SerializeField] private GameObject groundSmashPrefab;

    public int currentTile = 1;

    private GameObject[] explosions;
    private GameObject[] reflectFireball;
    private GameObject[] groundSmash;

    private void Awake()
    {
        explosions = new GameObject[tiles.Length];
        for (int i = 0; i < tiles.Length; i++)
            explosions[i] = Instantiate(explosionPrefab, tiles[i].transform);

        reflectFireball = new GameObject[tiles.Length];
        for (int i = 0; i < tiles.Length; i++)
            reflectFireball[i] = Instantiate(reflectFireballPrefab, tiles[i].transform);

        groundSmash = new GameObject[tiles.Length];
        for (int i = 0; i < tiles.Length; i++)
            groundSmash[i] = Instantiate(groundSmashPrefab, tiles[i].transform);
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
    public void SpawnIndicator(int num)
    {
        if (!CheckValidTile(num)) return;
        GameObject newAttackIndicator = Instantiate(attackIndicatorPrefab);
        attackIndicatorSpawners[num].AddObject(newAttackIndicator);
    }
    public void PopIndicator(int num)
    {
        if (!CheckValidTile(num)) return;
        attackIndicatorSpawners[num].RemoveObject().GetComponent<minoAttackIndicator>()?.OnTrigger();
    }

    public GameObject SpawnFireball(int num)
    {
        if (!CheckValidTile(num)) return null;

        GameObject newFireball = Instantiate(fireballPrefab);
        fireballSpawners[num].AddObject(newFireball);
        return newFireball;
    }

    public void ShootFireball(int num, GameObject fireball)
    {
        if (!CheckValidTile(num)) return;

        fireballSpawners[num].RemoveObject(fireball);
        fireball.GetComponent<Fireball>().OnShoot();
    }

    public void ActivateExplosion(int num)
    {
        if (CheckValidTile(num)) explosions[num].SetActive(true);
    }

    public void ActivateReflectFireball(int num)
    {
        if (CheckValidTile(num)) reflectFireball[num].SetActive(true);
    }

    public void ActivateGroundSmash(int num)
    {
        if (CheckValidTile(num)) groundSmash[num].SetActive(true);
    }
}
