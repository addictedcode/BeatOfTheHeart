using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateMinigameArrow : MonoBehaviour
{
    [SerializeField] private GameObject m_unlitArrow;
    [SerializeField] private SpriteRenderer m_litArrow;

    public void ToggleLitArrow(bool flag)
    {
        m_unlitArrow.SetActive(!flag);
        m_litArrow.enabled = flag;
    }

    public void ChangeDirection(Directions dir)
    {
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90 * ((int)dir - 1)));
    }
}
