using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(MusicFile music)
    {
        BeatsManager.BPM = music.BPM;
        audioSource.clip = music.Clip;
        audioSource.Play();
        MusicManager.OnPlayMusic.Invoke();
    }
}
