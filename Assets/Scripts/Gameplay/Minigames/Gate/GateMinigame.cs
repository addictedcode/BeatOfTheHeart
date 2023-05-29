using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Directions
{
    None, Up, Left, Down, Right
}

public class GateMinigame : MonoBehaviour
{
    [SerializeField] private List<DirectionsList> m_ComboList;
    [SerializeField] private List<GateMinigameBoard> m_BoardList;
    private int m_CurrentComboIndex = 0;
    private int m_CurrentComboListIndex = 0;

    bool m_MinigameFinished = false;

    [System.Serializable]
    struct DirectionsList
    {
        public Directions[] list;
    }

    public bool HasMinigame(int comboListIndex)
    {
        return m_ComboList.Count > comboListIndex;
    }

    public void PlayMinigame(int comboListIndex)
    {
        m_CurrentComboListIndex = comboListIndex;
        UpdateToMinigameControls();
        ResetMinigame();
        ResetCombo();
    }

    public void ResetMinigame()
    {
        m_MinigameFinished = false;
        m_BoardList[m_CurrentComboListIndex].gameObject.SetActive(true);
        m_BoardList[m_CurrentComboListIndex].ClearArrows();
        foreach (Directions dir in m_ComboList[m_CurrentComboListIndex].list)
        {
            m_BoardList[m_CurrentComboListIndex].GenerateArrow(dir);
        }
    }

    public void ResetCombo()
    {
        m_CurrentComboIndex = 0;
        m_BoardList[m_CurrentComboListIndex].UnlitArrows();
    }

    public IEnumerator UpdateMinigame()
    {
        while (!m_MinigameFinished)
        {
            if (m_CurrentComboIndex >= m_ComboList[m_CurrentComboListIndex].list.Length)
            {
                m_MinigameFinished = true;
                yield break;
            }

            yield return null;
        }
    }

    public void OnDirection(Vector2 value)
    {
        Directions direction = Directions.None;
        if (value.y >= 1)
        {
            direction = Directions.Up;
            GameManager.Instance.Player.AttackAnimation();
        }
        else if (value.y <= -1)
        {
            direction = Directions.Down;
            GameManager.Instance.Player.ReflectAnimation();
        }
        else if (value.x >= 1)
        {
            direction = Directions.Right;
            GameManager.Instance.Player.Move(1);
        }
        else if (value.x <= -1)
        {
            direction = Directions.Left;
            GameManager.Instance.Player.Move(-1);
        }
        CheckDirectionInput(direction);
    }

    private void CheckDirectionInput(Directions direction)
    {
        if (direction == Directions.None) return;
        if (m_CurrentComboIndex >= m_ComboList[m_CurrentComboListIndex].list.Length) return;

        if (direction == m_ComboList[m_CurrentComboListIndex].list[m_CurrentComboIndex])
        {
            m_BoardList[m_CurrentComboListIndex].LitArrow(m_CurrentComboIndex);
            SFXManager.Instance.PlayOneShot("Minigame_Correct");
            m_CurrentComboIndex++;
        }
        else
            OnFailedInput();
    }

    public void OnFailedInput()
    {
        ResetCombo();
        SFXManager.Instance.PlayOneShot("Miss");
    }

    public void UpdateToMinigameControls()
    {
        GameManager.Instance.PlayerInput.SwitchCurrentActionMap("GateMinigame");
    }

    public void OpenGate()
    {
        m_BoardList[m_CurrentComboListIndex].OpenGate();
    }
}
