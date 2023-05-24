using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateMinigameBoard : MonoBehaviour
{
    [SerializeField] private GameObject m_arrowPrefab;
    private List<GateMinigameArrow> m_arrows = new();

    public void GenerateArrow(Directions dir)
    {
        GameObject go = Instantiate(m_arrowPrefab, transform);
        GateMinigameArrow arrow = go.GetComponent<GateMinigameArrow>();
        arrow.ChangeDirection(dir);
        m_arrows.Add(arrow);
    }

    public void LitArrow(int index)
    {
        if (m_arrows.Count > index)
        {
            m_arrows[index].ToggleLitArrow(true);
        }
    }

    public void UnlitArrows()
    {
        foreach (GateMinigameArrow arrow in m_arrows)
        {
            arrow.ToggleLitArrow(false);
        }
    }

    public void ClearArrows()
    {
        foreach (GateMinigameArrow arrow in m_arrows)
        {
            Destroy(arrow.gameObject);
        }
        m_arrows.Clear();
    }
}
