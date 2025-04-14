using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : Singleton<SoundMixerManager>
{
    [SerializeField] private AudioMixer audioMixer;

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("MasterVolume", level == 0f ? -80f : Mathf.Log10(level) * 20f);

    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("MusicVolume", level == 0f ? -80f : Mathf.Log10(level) * 20f);
    }

    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("SoundFXVolume", level == 0f ? -80f : Mathf.Log10(level) * 20f);
    }

    public void SetAmbianceVolume(float level)
    {
        audioMixer.SetFloat("AmbianceVolume", level == 0f ? -80f : Mathf.Log10(level) * 20f);

    }
}
