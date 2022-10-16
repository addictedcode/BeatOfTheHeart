using System;
using UnityEngine;

public static class MusicManager
{
    public static Action OnPlayMusic;
    public static Action OnStopMusic;

    public static AudioSource audioSource;
    public static MusicPlayer player;
}
