using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateMinigame : MonoBehaviour
{
    [SerializeField] private List<DirectionsList> m_ComboList;
    private int m_CurrentComboIndex = 0;
    private int m_CurrentComboListIndex = 0;

    bool m_MinigameFinished = false;

    [System.Serializable]
    struct DirectionsList
    {
        public Directions[] list;
    }

    enum Directions
    {
        None, Up, Down, Left, Right
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
    }

    public void ResetCombo()
    {
        m_CurrentComboIndex = 0;
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
        }
        else if (value.y <= -1)
        {
            direction = Directions.Down;
        }
        else if (value.x >= 1)
        {
            direction = Directions.Right;
        }
        else if (value.x <= -1)
        {
            direction = Directions.Left;
        }
        CheckDirectionInput(direction);
    }

    private void CheckDirectionInput(Directions direction)
    {
        if (direction == Directions.None) return;
        if (m_CurrentComboIndex >= m_ComboList[m_CurrentComboListIndex].list.Length) return;

        if (direction == m_ComboList[m_CurrentComboListIndex].list[m_CurrentComboIndex])
        {
            m_CurrentComboIndex++;
            Debug.Log(direction);
        }
        else
            OnFailedInput();
    }

    public void OnFailedInput()
    {
        ResetCombo();
        Debug.Log("Failed");
    }

    public void UpdateToMinigameControls()
    {
        GameManager.Instance.PlayerInput.SwitchCurrentActionMap("GateMinigame");
    }
}
