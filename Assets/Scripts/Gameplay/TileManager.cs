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

    public void MoveToTile(int dir)
    {
        if (CheckValidSideTile(dir))
        {
            GameManager.Instance.Player.transform.position = tiles[currentTile += dir].transform.position;
            GameManager.Instance.Player.flipSprite(currentTile == 1);
        }
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

    public void ClearAll()
    {
        attackIndicatorSpawners[0].ClearAll();
        attackIndicatorSpawners[1].ClearAll();
        fireballSpawners[0].ClearAll();
        fireballSpawners[1].ClearAll();
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

    public void SpawnExplosion(int num)
    {
        if (!CheckValidTile(num)) return;
        Instantiate(explosionPrefab, tiles[num].transform);
    }

    public void ActivateReflectFireball(int num)
    {
        if (CheckValidTile(num))
        {
            Instantiate(reflectFireballPrefab, tiles[num].transform).GetComponent<Fireball>().OnShoot();
        }
    }

    public void SpawnGroundSmash(int num)
    {
        if (!CheckValidTile(num)) return;
        Instantiate(groundSmashPrefab, tiles[num].transform);
    }
}
