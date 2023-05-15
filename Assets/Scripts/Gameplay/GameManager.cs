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
    //[SerializeField] private Minotaur minotaur;

    private TileManager tileManager;
    //private Forte player;
    private Minotaur minotaur;

    private PhasesManager phasesManager;

    [Header("End Screen")]
    [SerializeField] private GameObject endCanvas;
    [SerializeField] private GameObject victoryText;
    [SerializeField] private GameObject defeatText;

    public Forte Player => player;
    public Minotaur Minotaur => minotaur;
    public TileManager TileManager => tileManager;
    public UnityEngine.InputSystem.PlayerInput PlayerInput => player.GetComponent<UnityEngine.InputSystem.PlayerInput>();
    public GameState GameState;

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

    public void ActivateIndicator(int num) => tileManager.ActivateIndicator(num);
    public void ActivateFireball(int num) => tileManager.ActivateFireball(num);
    public void ActivateExplosion(int num) => tileManager.ActivateExplosion(num);
    public void ActivateReflectFireball(int num) => tileManager.ActivateReflectFireball(num);
    public void ActivateGroundSmash(int num) => tileManager.ActivateGroundSmash(num);
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
        phasesManager.Phases[phasesManager.currentPhase].camera.transform.SetParent(player.transform);
        player.PlayAnimation("PC_Transition1"); 
        phasesManager.currentPhase++;

        StartCoroutine(Delay());

        IEnumerator Delay()
        {
            yield return new WaitForSeconds(5);

            PlayGameAfterDelay(2.0f); // to accomodate for Minotaur's death animation
        }
    }

    public void EndCombat(bool isVictory)
    {

        //MusicManager.player.StopMusic();
        //SFXManager.Instance.PlayOneShot("Lose");
        PlayerInput.enabled = false;
        phasesManager.currentPhaseState = PhaseState.CombatEnd;

        if (isVictory)
        {
            if (phasesManager.currentPhase + 1 < phasesManager.Phases.Count)
            {
                //transition logic here
                StartCoroutine(Delay());

                IEnumerator Delay()
                {
                    yield return new WaitForSeconds(2.0f);
                    StartTransition();
                }
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
