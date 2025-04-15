using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_TempoToggle : MonoBehaviour
{
    [SerializeField] private bool _isPast;

    [Header("VFX Sets")]
    [SerializeField] private List<ParticleSystem> vfxSetPast;
    [SerializeField] private List<ParticleSystem> vfxSetFuture;

    private ChangeTime _changeTime;

    private void Start()
    {
        _changeTime = GameManager.Instance.Character.GetComponent<ChangeTime>();
        _changeTime.OnTimeChangeEnd += OnChangedTime;

        SetVFXInstant(_isPast);
    }

    private void OnChangedTime(bool isNowPast)
    {
        if (isNowPast == _isPast)
        {
            PlayVFX(vfxSetPast);
            StopVFX(vfxSetFuture);
        }
        else
        {
            PlayVFX(vfxSetFuture);
            StopVFX(vfxSetPast);
        }
    }

    private void SetVFXInstant(bool isPast)
    {
        if (isPast)
        {
            PlayVFX(vfxSetPast);
            StopVFX(vfxSetFuture);
        }
        else
        {
            PlayVFX(vfxSetFuture);
            StopVFX(vfxSetPast);
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
            _changeTime.OnTimeChangeEnd -= OnChangedTime;
    }
}
