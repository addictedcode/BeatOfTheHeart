using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularSpawner : MonoBehaviour
{
    [SerializeField] private float m_DistanceFromCenter = 2.0f;

    private List<GameObject> m_ObjectList = new List<GameObject>();

    public void AddObject(GameObject newObject)
    {
        m_ObjectList.Add( newObject );
        newObject.transform.SetParent(transform);
        AdjustCircle();
    }

    public void RemoveObject(GameObject gameObject)
    {
        m_ObjectList.Remove( gameObject );
        AdjustCircle();
    }

    private void AdjustCircle()
    {
        float AngleStep = 360.0f / m_ObjectList.Count;

        //If object is only 1, set the position to center
        if (m_ObjectList.Count == 1)
        {
            m_ObjectList[0].transform.position = transform.position;
            return;
        }

        for (int i = 0; i < m_ObjectList.Count; i++)
        {
            m_ObjectList[i].transform.position = new Vector3(transform.position.x + m_DistanceFromCenter, transform.position.y, transform.position.z);
            m_ObjectList[i].transform.RotateAround(transform.position, Vector3.forward, AngleStep * ( i + 1 ));
        }
    }
}
