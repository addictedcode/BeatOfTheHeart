using UnityEngine;

[CreateAssetMenu(fileName = "SFXFile", menuName = "SFX/SFXFile", order = 1)]
public class SFXFile : ScriptableObject
{
    [Header("SFX")]
    public AudioClip clip;

    [Header("Settings")]
    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    public bool loop;

    [HideInInspector]
    public AudioSource source;
}