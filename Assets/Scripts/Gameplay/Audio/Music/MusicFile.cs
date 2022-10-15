using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicFile", menuName = "Music/MusicFile", order = 1)]
public class MusicFile : ScriptableObject
{
    public AudioClip Clip;
    public uint BPM;
}
