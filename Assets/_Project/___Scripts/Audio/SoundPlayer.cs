using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float _minPitch = 0.85f;
    [SerializeField] private float _maxPitch = 1.15f;
    [SerializeField] private float _minVolume = 0.75f;
    [SerializeField] private float _maxVolume = 1f;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    public void PlayWithRandomPitchAndVolume()
    {
        if (_audioClip == null)
        {
            Debug.LogWarning("No AudioClip assigned to SoundPlayer on " + gameObject.name);
            return;
        }

        _audioSource.pitch = Random.Range(_minPitch, _maxPitch);
        _audioSource.volume = Random.Range(_minVolume, _maxVolume);
        _audioSource.PlayOneShot(_audioClip);
    }
}
