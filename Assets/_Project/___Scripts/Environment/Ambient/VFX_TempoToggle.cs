using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_TempoToggle : MonoBehaviour
{
    [SerializeField] private bool _isPast;

    [Header("VFX Sets")]
    [SerializeField] private List<ParticleSystem> _vfxSetPast;
    [SerializeField] private List<ParticleSystem> _vfxSetPresent;

    private ChangeTime _changeTime;

    private void Start()
    {
        _changeTime = GameManager.Instance.Character.GetComponent<ChangeTime>();
        _changeTime.OnTimeChangeStarted+= OnChangedTime;

        SetVFXInstant(_isPast);
    }

    private void OnChangedTime(bool isNowPast)
    {
        SetVFXInstant(!isNowPast);
    }

    private void SetVFXInstant(bool toPast)
    {
        if (toPast)
        {
            PlayVFX(_vfxSetPast);
            StopVFX(_vfxSetPresent);
        }
        else
        {
            PlayVFX(_vfxSetPresent);
            StopVFX(_vfxSetPast);
        }
    }

    private void PlayVFX(List<ParticleSystem> vfxList)
    {
        foreach (var vfx in vfxList)
        {
            if (vfx != null && !vfx.isPlaying)
                vfx.Play();
        }
    }

    private void StopVFX(List<ParticleSystem> vfxList)
    {
        foreach (var vfx in vfxList)
        {
            if (vfx != null && vfx.isPlaying)
                vfx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    private void OnDestroy()
    {
        if (_changeTime != null)
            _changeTime.OnTimeChangeStarted -= OnChangedTime;
    }
}
