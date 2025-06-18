using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UiSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _audioclip;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.clip = _audioclip;
    }

    public void PlaySound()
    {
        _audioSource.Play();
    }
}
