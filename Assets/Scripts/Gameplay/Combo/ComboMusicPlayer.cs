using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ComboMusicPlayer : MonoBehaviour
{
    [System.Serializable]
    public class ComboMusic
    {
        public AudioClip clip;
        public int comboCount;
        [HideInInspector] public AudioSource audioSource;
    }

    [SerializeField] private List<ComboMusic> comboMusicList;
    [SerializeField] private AudioMixerGroup mixerGroup;

    private void Awake()
    {
        foreach (ComboMusic comboMusic in comboMusicList) 
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = .4f;
            audioSource.clip = comboMusic.clip;
            audioSource.loop = true;
            audioSource.outputAudioMixerGroup = mixerGroup;
            audioSource.playOnAwake = false;
            comboMusic.audioSource = audioSource;
        }
    }

    public void OnComboIncrement(int currentCombo)
    {
        foreach (ComboMusic comboMusic in comboMusicList)
        {
            if (!comboMusic.audioSource.isPlaying && comboMusic.comboCount <= currentCombo)
            {
                comboMusic.audioSource.time = MusicManager.audioSource.time;
                comboMusic.audioSource.Play();
            }
        }
    }

    public void OnComboReset()
    {
        foreach (ComboMusic comboMusic in comboMusicList)
        {
            comboMusic.audioSource.Stop();
        }
    }

    
}
