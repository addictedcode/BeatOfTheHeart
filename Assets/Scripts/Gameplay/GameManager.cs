using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TileManager tileManager;
    [SerializeField] private Forte player;
    [SerializeField] private Minotaur minotaur;

    [Header("End Screen")]
    [SerializeField] private GameObject endCanvas;
    [SerializeField] private GameObject victoryText;
    [SerializeField] private GameObject defeatText;

    public Forte Player => player;
    public Minotaur Minotaur => minotaur;
    public TileManager TileManager => tileManager;
    public UnityEngine.InputSystem.PlayerInput PlayerInput => player.GetComponent<UnityEngine.InputSystem.PlayerInput>();

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
    public void ActivateFireball(int num) => tileManager.ActivateFireball(num);
    public void ActivateExplosion(int num) => tileManager.ActivateExplosion(num);
    public void ActivateReflectFireball(int num) => tileManager.ActivateReflectFireball(num);
    public void ActivateGroundSmash(int num) => tileManager.ActivateGroundSmash(num);
    #endregion

    public void PlayGameAfterDelay(float delay)
    {
        PlayerInput.enabled = false;
        StartCoroutine(Delay());
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(delay);
            minotaur.StartMinotaur();
            PlayerInput.enabled = true;
        }
    }

    public void EndGame(bool isVictory)
    {
        MusicManager.player.StopMusic();
        SFXManager.Instance.PlayOneShot("Lose");
        PlayerInput.enabled = false;

        if (!isVictory)
            minotaur.PlayerDeath();

        StartCoroutine(EndScreen());

        IEnumerator EndScreen()
        {
            yield return new WaitForSeconds(2);
            endCanvas.SetActive(true);
            if (isVictory)
                victoryText.SetActive(true);
            else
                defeatText.SetActive(true);
        }
    }
}
