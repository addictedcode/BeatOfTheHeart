using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    //TEMP
    [SerializeField] private MusicFile musicFile;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        MusicManager.audioSource = audioSource;
    }

    private void Start()
    {
        PlayMusic(musicFile);
    }

    public void PlayMusic(MusicFile music)
    {
        BeatsManager.BPM = music.BPM;
        audioSource.clip = music.Clip;
        audioSource.Play();
        MusicManager.OnPlayMusic.Invoke();
    }
}
