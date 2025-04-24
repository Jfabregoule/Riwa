using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager<T> : Singleton<T> where T : SoundMixerManager<T>
{
    [SerializeField] protected AudioMixer _audioMixer;

    public void SetMasterVolume(float level)
    {
        _audioMixer.SetFloat("MasterVolume", level == 0f ? -80f : Mathf.Log10(level) * 20f);

    }

    public void SetMusicVolume(float level)
    {
        _audioMixer.SetFloat("MusicVolume", level == 0f ? -80f : Mathf.Log10(level) * 20f);
    }

    public void SetSoundFXVolume(float level)
    {
        _audioMixer.SetFloat("SoundFXVolume", level == 0f ? -80f : Mathf.Log10(level) * 20f);
    }

    public void SetCinematicVolume(float level)
    {
        _audioMixer.SetFloat("CinematicVolume", level == 0f ? -80f : Mathf.Log10(level) * 20f);
    }
}
