using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        mainMixer.GetFloat("MasterVolume", out float master);
        masterSlider.value = ConvertDecibelValueToLinear(master);

        mainMixer.GetFloat("MusicVolume", out float music);
        musicSlider.value = ConvertDecibelValueToLinear(music);

        mainMixer.GetFloat("SFXVolume", out float sfx);
        sfxSlider.value = ConvertDecibelValueToLinear(sfx);
    }

    public void SetMasterVolume(float volume)
    {
        mainMixer.SetFloat("MasterVolume", ConvertLinearToDecibelValue(volume));
    }

    public void SetMusicVolume(float volume)
    {
        mainMixer.SetFloat("MusicVolume", ConvertLinearToDecibelValue(volume));
    }

    public void SetSFXVolume(float volume)
    {
        mainMixer.SetFloat("SFXVolume", ConvertLinearToDecibelValue(volume));
    }

    public static float ConvertLinearToDecibelValue(float lin)
    {
        return Mathf.Log10(lin) * 20;
    }

    public static float ConvertDecibelValueToLinear(float dec)
    {
        return Mathf.Pow(10, dec / 20);
    }
}
