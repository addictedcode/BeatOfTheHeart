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
        MusicManager.audioSource = audioSource;
        MusicManager.player = this;
    }

    public void PlayMusic(MusicFile music)
    {
        BeatsManager.BPM = music.BPM;
        audioSource.clip = music.Clip;
        audioSource.Play();
        MusicManager.OnPlayMusic.Invoke();
    }

    public IEnumerator FadeMusic(string name, float duration, float targetVolume)
    {
        float currentTime = 0;
        float startVolume = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
