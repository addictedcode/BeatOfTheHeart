using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer mainMixer;

    private void Start()
    {
        var slider = GetComponent<Slider>();
        float volume;
        mainMixer.GetFloat("MasterVolume", out volume);
        volume = Mathf.Pow(10, volume / 20);
        slider.value = volume;
    }

    public void SetVolume(float volume)
    {
        mainMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }
}
