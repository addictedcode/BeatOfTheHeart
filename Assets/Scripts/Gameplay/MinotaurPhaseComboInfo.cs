using UnityEngine;

[System.Serializable]
public struct MinotaurPhaseComboInfo
{
    public MinotaurComboList comboList;
    [Range (0, 1)] public float HPThreshold;
}
