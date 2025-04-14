using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_TempoToggle : MonoBehaviour
{
    [SerializeField] private bool _isPast;

    private ParticleSystem _particleSystem;
    private ChangeTime _changeTime;

    private void Start()
    {
        _changeTime = GameManager.Instance.Character.GetComponent<ChangeTime>();
        _changeTime.OnTimeChangeEnd += OnChangedTime;
    }

    private void OnChangedTime(bool isPast)
    {
        if (_isPast == isPast)
        {
            _particleSystem.Play();
        }
        else
        {
            _particleSystem.Stop();
        }
    }
}
