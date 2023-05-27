using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Playing,
    GameOver,
    Paused
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //[SerializeField] private TileManager tileManager;
    [SerializeField] private Forte player;
    [SerializeField] private GateMinigame gateMinigame;
    //[SerializeField] private Minotaur minotaur;

    private TileManager tileManager;
    //private Forte player;
    private Minotaur minotaur;

    private PhasesManager phasesManager;

    [Header("End Screen")]
    [SerializeField] private GameObject endCanvas;
    [SerializeField] private GameObject victoryText;
    [SerializeField] private GameObject defeatText;

    [Header("Game Settings")]
    [SerializeField] private PlayerComboSettingsSO playerComboSettings;
    [SerializeField] private ComboMusicPlayer comboMusicPlayer;
    
    public Forte Player => player;
    public Minotaur Minotaur => minotaur;
    public TileManager TileManager => tileManager;
    public UnityEngine.InputSystem.PlayerInput PlayerInput => player.GetComponent<UnityEngine.InputSystem.PlayerInput>();
    public GameState GameState;
    
    public int PlayerComboCount { get; private set; }
    public int PlayerComboCurrentLevel { get; private set; }
    
    public static Action OnComboMeterUpdated;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
        DontDestroyOnLoad(gameObject);


        phasesManager = PhasesManager.Instance;
    }

    #region Minotaur
    public void CheckPlayerTakeDamage(int damage, int tile)
    {
        if (tileManager.currentTile == tile)
            player.TakeDamage(damage);
    }

    public void SpawnIndicator(int num) => tileManager.SpawnIndicator(num);
    public void TriggerIndicator(int num) => tileManager.PopIndicator(num);
    public GameObject SpawnFireball(int num) => tileManager.SpawnFireball(num);
    public void ShootFireball(int num, GameObject fireball) => tileManager.ShootFireball(num, fireball);
    public void SpawnExplosion(int num) => tileManager.SpawnExplosion(num);
    public void ActivateReflectFireball(int num) => tileManager.ActivateReflectFireball(num);
    public void SpawnGroundSmash(int num) => tileManager.SpawnGroundSmash(num);
    #endregion
    
    #region Combo Meter

    public void IncrementComboCount()
    {
        PlayerComboCount++;

        if (!playerComboSettings.CheckIfAtMaxLevel(PlayerComboCurrentLevel))
        {
            PlayerComboLevel level = playerComboSettings.Levels[PlayerComboCurrentLevel];
            if (PlayerComboCount > level.Threshold)
                PlayerComboCurrentLevel++;
        }

        comboMusicPlayer.OnComboIncrement(PlayerComboCount);
        OnComboMeterUpdated?.Invoke();
    }

    public void ResetComboMeter()
    {
        PlayerComboCount = 0;
        PlayerComboCurrentLevel = 0;

        comboMusicPlayer.OnComboReset();
        OnComboMeterUpdated?.Invoke();
    }

    public float GetPlayerComboDamageMultiplier()
    {
        bool atMaxLevel = playerComboSettings.CheckIfAtMaxLevel(PlayerComboCurrentLevel);
        return atMaxLevel ? playerComboSettings.MaxDamageMultiplier : playerComboSettings.Levels[PlayerComboCurrentLevel].DamageMultipler;
    }
    
    #endregion

    public void PlayGameAfterDelay(float delay)
    {
        PlayerInput.enabled = false;
        phasesManager.InitPhase(phasesManager.currentPhase);
        phasesManager.currentPhaseState = PhaseState.Spawning;

        // delayed to allow for spawn animation to play
        StartCoroutine(Delay());
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(delay);
            phasesManager.currentPhaseState = PhaseState.Combat;
            PlayerInput.enabled = true;
            minotaur.StartCombat();
        }
    }

    public void UpdatePhase(Minotaur newMinotaur, TileManager newTiles)
    {
        minotaur = newMinotaur;
        tileManager = newTiles;
    }

    public void StartTransition()
    {
        phasesManager.currentPhaseState = PhaseState.Transition;
        //phasesManager.Phases[phasesManager.currentPhase].camera.transform.SetParent(player.transform);
        player.PlayAnimation("PC_Transition1"); 
        phasesManager.currentPhase++;

        StartCoroutine(Delay());

        IEnumerator Delay()
        {
            yield return new WaitForSeconds(5);

            PlayGameAfterDelay(3.0f); // to accomodate for Minotaur's death animation
        }
    }

    public IEnumerator StartMinigame()
    {
        // accomodate for mino's death anim
        yield return new WaitForSeconds(2.0f);

        if (gateMinigame.HasMinigame(PhasesManager.Instance.currentPhase))
        {
            // play an animation here for establishing gate minigame
            yield return new WaitForSeconds(1.0f);
            if (phasesManager.Phases[phasesManager.currentPhase].board != null)
                phasesManager.Phases[phasesManager.currentPhase].camera.LookAt = phasesManager.Phases[phasesManager.currentPhase].board.transform;

            
            yield return new WaitForSeconds(1.0f);
            gateMinigame.PlayMinigame(PhasesManager.Instance.currentPhase);

            yield return gateMinigame.UpdateMinigame();
            gateMinigame.OpenGate();
            PlayerInput.SwitchCurrentActionMap("Player");
        }

        PlayerInput.enabled = false;
        yield return new WaitForSeconds(2.0f);
        StartTransition();
    }

    public void EndCombat(bool isVictory)
    {

        //MusicManager.player.StopMusic();
        //SFXManager.Instance.PlayOneShot("Lose");
        phasesManager.currentPhaseState = PhaseState.CombatEnd;

        if (isVictory)
        {
            if (phasesManager.currentPhase + 1 < phasesManager.Phases.Count)
            {
                //transition logic here
                StartCoroutine(StartMinigame());
            }
            else
            {
                EndGame(isVictory);
            }

        }
        else
        {
            EndGame(isVictory);
        }
    }
    public void EndGame(bool isVictory)
    {

        //MusicManager.player.StopMusic();
        //SFXManager.Instance.PlayOneShot("Lose");
        PlayerInput.enabled = false;

        if (isVictory)
        {
            StartCoroutine(EndScreen());

            IEnumerator EndScreen()
            {
                yield return new WaitForSeconds(2);
                endCanvas.SetActive(true);
                victoryText.SetActive(true);
            }
        }
        else
        {
            StartCoroutine(EndScreen());

            IEnumerator EndScreen()
            {
                yield return new WaitForSeconds(2);
                endCanvas.SetActive(true);
                defeatText.SetActive(true);
            }
        }

        phasesManager.currentPhase = 0;
    }

}
