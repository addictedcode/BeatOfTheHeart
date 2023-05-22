using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingSpawner : MonoBehaviour
{
    [SerializeField] private float m_ScaleIncreaseIncrement = 0.25f;

    private Stack<GameObject> m_ObjectList = new Stack<GameObject>();

    public void AddObject(GameObject newObject)
    {
        m_ObjectList.Push( newObject );
        newObject.transform.SetParent(transform);
        newObject.transform.localPosition = Vector3.zero;
        newObject.transform.localScale = new Vector3(
            m_ScaleIncreaseIncrement * m_ObjectList.Count, 
            m_ScaleIncreaseIncrement * m_ObjectList.Count, 
            m_ScaleIncreaseIncrement * m_ObjectList.Count);
    }

    public GameObject RemoveObject()
    {
        return m_ObjectList.Pop();
    }
}
