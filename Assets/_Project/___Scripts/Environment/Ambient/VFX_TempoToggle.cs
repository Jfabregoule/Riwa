using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_TempoToggle : MonoBehaviour
{
    [SerializeField] private bool _isPast;

    [Header("VFX Sets")]
    [SerializeField] private List<ParticleSystem> _vfxSetPast;
    [SerializeField] private List<ParticleSystem> _vfxSetPresent;

    private void Start()
    {
        SetVFXInstant(GameManager.Instance.CurrentTemporality);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnTimeChangeStarted += SetVFXInstant;
    }

    private void OnDisable()
    {
        if (GameManager.Instance)
            GameManager.Instance.OnTimeChangeStarted -= SetVFXInstant;
    }

    private void SetVFXInstant(EnumTemporality temporality)
    {
        if (temporality == EnumTemporality.Past)
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
}
