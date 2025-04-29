using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager<T> : Singleton<T> where T : SoundMixerManager<T>
{
    [SerializeField] protected AudioMixer _audioMixer;

    public void SetMasterVolume(float level)
    {
        float normalized = Mathf.Clamp01(level / 10f);
        float dB = normalized == 0f ? -80f : Mathf.Log10(normalized) * 20f;
        _audioMixer.SetFloat("MasterVolume", dB);
    }

    public void SetMusicVolume(float level)
    {
        float normalized = Mathf.Clamp01(level / 10f);
        float dB = normalized == 0f ? -80f : Mathf.Log10(normalized) * 20f;
        _audioMixer.SetFloat("MusicVolume", dB);
    }

    public void SetSoundFXVolume(float level)
    {
        float normalized = Mathf.Clamp01(level / 10f);
        float dB = normalized == 0f ? -80f : Mathf.Log10(normalized) * 20f;
        _audioMixer.SetFloat("SoundFXVolume", dB);
    }
    public void SetAmbiantVolume(float level)
    {
        float normalized = Mathf.Clamp01(level / 10f);
        float dB = normalized == 0f ? -80f : Mathf.Log10(normalized) * 20f;
        _audioMixer.SetFloat("AmbianceVolume", dB);
    }

    public void SetCinematicVolume(float level)
    {
        float normalized = Mathf.Clamp01(level / 10f);
        float dB = normalized == 0f ? -80f : Mathf.Log10(normalized) * 20f;
        _audioMixer.SetFloat("CinematicVolume", dB);
    }
}
