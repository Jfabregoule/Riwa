using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class RiwaSoundMixerManager : SoundMixerManager<RiwaSoundMixerManager>
{
    [SerializeField] private float _mixerBlendDuration = 1.0f;
    [Range(0f, 1f)]
    [SerializeField] private float _blendMidPoint = 0.5f;

    private void Start()
    {
        BlendToTemporality(GameManager.Instance.CurrentTemporality);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnTimeChangeStarted += BlendToTemporality;
    }

    private void OnDisable()
    {
        if (GameManager.Instance)
            GameManager.Instance.OnTimeChangeStarted -= BlendToTemporality;
    }

    public void SetMusicVolume(float level, Temporality temporality)
    {
        _audioMixer.SetFloat($"MusicVolume{temporality}", level == 0f ? -80f : Mathf.Log10(level) * 20f);
    }

    public void SetSoundFXVolume(float level, Temporality temporality)
    {
        _audioMixer.SetFloat($"SoundFXVolume{temporality}", level == 0f ? -80f : Mathf.Log10(level) * 20f);
    }

    public void SetAmbianceVolume(float level, Temporality temporality)
    {
        _audioMixer.SetFloat($"AmbianceVolume{temporality}", level == 0f ? -80f : Mathf.Log10(level) * 20f);
    }

    public void BlendToTemporality(Temporality toTemporality)
    {
        Temporality fromTemporality = toTemporality == Temporality.Past ? Temporality.Present : Temporality.Past;

        StartCoroutine(FadeMixerGroupVolume($"Music{fromTemporality}Volume", 0f, _mixerBlendDuration, false));
        StartCoroutine(FadeMixerGroupVolume($"Music{toTemporality}Volume", 1f, _mixerBlendDuration, true));

        StartCoroutine(FadeMixerGroupVolume($"Ambiance{fromTemporality}Volume", 0f, _mixerBlendDuration, false));
        StartCoroutine(FadeMixerGroupVolume($"Ambiance{toTemporality}Volume", 1f, _mixerBlendDuration, true));

        StartCoroutine(FadeMixerGroupVolume($"SoundFX{fromTemporality}Volume", 0f, _mixerBlendDuration, false));
        StartCoroutine(FadeMixerGroupVolume($"SoundFX{toTemporality}Volume", 1f, _mixerBlendDuration, true));

    }

    /// <summary>
    /// Coroutine qui fade un paramètre entre deux points définis en pourcentage (startPercent à endPercent).
    /// Exemple : 0 à 0.5 pour fade-out, 0.5 à 1 pour fade-in.
    /// </summary>
    private IEnumerator FadeMixerGroupVolume(string exposedParam, float targetLinearVolume, float duration, bool fadeIn)
    {
        float targetDb = targetLinearVolume == 0f ? -80f : Mathf.Log10(targetLinearVolume) * 20f;

        if (!_audioMixer.GetFloat(exposedParam, out float currentDb))
            currentDb = targetDb;

        float startDb = currentDb;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            float curvedT = fadeIn
                ? Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0f, 1f, t + (_blendMidPoint - 0.5f)))
                : Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0f, 1f, t - (_blendMidPoint - 0.5f)));

            float newDb = Mathf.Lerp(startDb, targetDb, curvedT);
            _audioMixer.SetFloat(exposedParam, newDb);

            yield return null;
        }

        _audioMixer.SetFloat(exposedParam, targetDb);
    }

}
