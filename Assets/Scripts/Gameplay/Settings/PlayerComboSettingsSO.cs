using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerComboLevel
{
    [SerializeField] private int threshold;
    [SerializeField] private float damageMultipler;
    [SerializeField] private Color meterColor;
    
    public int Threshold => threshold;
    public float DamageMultipler => damageMultipler;
    public Color MeterColor => meterColor;
}

[CreateAssetMenu(fileName = "Player Combo Settings", menuName = "Settings/PlayerCombo")]
public class PlayerComboSettingsSO : ScriptableObject
{
    [Header("General Settings")]
    [SerializeField] private List<PlayerComboLevel> levels;
    public IReadOnlyList<PlayerComboLevel> Levels => levels;
    public int LevelCount => Levels.Count;
    public bool CheckIfAtMaxLevel(int level) => level >= Levels.Count;

    [Header("Miss Settings")]
    [SerializeField] private bool hasUniqueTextOnMiss;
    [SerializeField] private string uniqueMissText;
    [SerializeField] private Color missComboColor;
    public bool HasUniqueTextOnMiss => hasUniqueTextOnMiss;
    public string UniqueMissText => uniqueMissText;
    public Color MissComboColor => missComboColor;
    
    [Header("Max Settings")]
    [SerializeField] private bool hasUniqueTextOnMax;
    [SerializeField] private string uniqueMaxText;
    [SerializeField] private Color maxComboColor;
    [SerializeField] private float maxDamageMultiplier;

    public bool HasUniqueTextOnMax => hasUniqueTextOnMax;
    public string UniqueMaxText => uniqueMaxText;
    public Color MaxComboColor => maxComboColor;
    public float MaxDamageMultiplier => maxDamageMultiplier;

}
