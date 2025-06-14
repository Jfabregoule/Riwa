using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LoopedSoundPlayer : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float fadeInTime = 1.0f;
    [SerializeField] private float fadeOutTime = 1.0f;
    [SerializeField] private float maxVolume = 1.0f;

    private AudioSource _audioSource;
    private Coroutine _currentFade;
    private bool _isPlaying;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;
        _audioSource.clip = _audioClip;
        _audioSource.volume = 0f;
    }

    public void Play()
    {
        if (_isPlaying) return;

        if (_currentFade != null)
            StopCoroutine(_currentFade);

        _audioSource.Play();
        _currentFade = StartCoroutine(FadeIn());
        _isPlaying = true;
    }

    public void Stop()
    {
        if (!_isPlaying) return;

        if (_currentFade != null)
            StopCoroutine(_currentFade);

        _currentFade = StartCoroutine(FadeOut());
        _isPlaying = false;
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;

        while (elapsed < fadeInTime)
        {
            _audioSource.volume = Mathf.Lerp(0f, maxVolume, elapsed / fadeInTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _audioSource.volume = maxVolume;
    }

    private IEnumerator FadeOut()
    {
        float startVolume = _audioSource.volume;
        float elapsed = 0f;

        while (elapsed < fadeOutTime)
        {
            _audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeOutTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _audioSource.volume = 0f;
        _audioSource.Stop();
    }

    public bool IsPlaying => _isPlaying;
}
