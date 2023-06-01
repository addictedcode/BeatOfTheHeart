using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public enum GameState
{
    Playing,
    GameOver,
    Paused,
    MainMenu
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
    public PhasesManager PM => phasesManager;

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

    [SerializeField] private GameObject GameLights;
    [SerializeField] private GameObject AnimatorParent;
    public CinemachineVirtualCamera currentVC;


    public static Action OnComboMeterUpdated;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
        DontDestroyOnLoad(gameObject);

        GameState = GameState.MainMenu;
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


    public void StartGameplay(float delay, CinemachineVirtualCamera mainMenuVC)
    {
        GameState = GameState.Playing;
        player.gameObject.SetActive(true);
        // play transition to gameplay here
        AnimatorParent.GetComponent<Animator>().enabled = true;
        currentVC = mainMenuVC;
        currentVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1;

        // delayed to allow for spawn animation to play
        StartCoroutine(Delay());

        IEnumerator Delay()
        {
            yield return new WaitForSeconds(2.0f);
            currentVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;

            mainMenuVC.Priority = 0;

            PlayGameAfterDelay(delay);
        }
    }
    public void PlayGameAfterDelay(float delay)
    {

        player.gameObject.SetActive(true);
        GameLights.SetActive(true);
        PlayerInput.enabled = false;
        phasesManager.InitPhase(phasesManager.currentPhase);
        currentVC = phasesManager.Phases[phasesManager.currentPhase].camera;
        phasesManager.currentPhaseState = PhaseState.Spawning;
        player.GetComponent<Animator>().applyRootMotion = false;
        switch (phasesManager.currentPhase)
        {
            case 0:
                player.PlayAnimation("LandingTransition1");
                break;
            case 1:
                player.PlayAnimation("LandingTransition2");
                break;
            case 2:
                player.PlayAnimation("LandingTransition3");
                break;
            default:
                break;
        }


        // delayed to allow for spawn animation to play
        StartCoroutine(Delay());
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(delay);
            player.PlayAnimation("Idle");
            player.GetComponent<Animator>().applyRootMotion = true;
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
        player.PlayAnimation("JumpTransition"); 
        

        StartCoroutine(Delay());

        IEnumerator Delay()
        {
            yield return new WaitForSeconds(1.25f);
            PlayGameAfterDelay(4.5f); // to accomodate for Minotaur's death animation
        }
    }

    public IEnumerator StartMinigame()
    {
        // accomodate for mino's death anim
        yield return new WaitForSeconds(2.0f);

        int previousPhase = PhasesManager.Instance.currentPhase;

        if (gateMinigame.HasMinigame(PhasesManager.Instance.currentPhase))
        {
            // play an animation here for establishing gate minigame
            yield return new WaitForSeconds(1.0f);
            if (phasesManager.Phases[phasesManager.currentPhase].board != null)
                phasesManager.Phases[phasesManager.currentPhase].camera.LookAt = phasesManager.Phases[phasesManager.currentPhase].board.transform;

            
            yield return new WaitForSeconds(1.0f);
            gateMinigame.PlayMinigame(PhasesManager.Instance.currentPhase);

            yield return gateMinigame.UpdateMinigame();
            phasesManager.currentPhase++;
            gateMinigame.OpenGate();
            SFXManager.Instance.PlayOneShot("Open_Gate_Sound");
            PlayerInput.SwitchCurrentActionMap("Player");
        }

        PlayerInput.enabled = false;
        yield return new WaitForSeconds(2.0f);

        if (phasesManager.currentPhase < phasesManager.Phases.Count)
        {
            //transition logic here
            StartTransition();
        }
        else
        {
            phasesManager.Phases[previousPhase].minotaur.GetComponent<Animator>().Play("Death3");
            EndGame(true);
        }
        
    }

    public void EndCombat(bool isVictory)
    {

        //MusicManager.player.StopMusic();
        //SFXManager.Instance.PlayOneShot("Lose");
        phasesManager.currentPhaseState = PhaseState.CombatEnd;

        if (isVictory)
        {
            StartCoroutine(StartMinigame());
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
