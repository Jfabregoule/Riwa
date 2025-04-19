using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Volume_TempoToggle : MonoBehaviour
{
    [SerializeField] private bool _isPast;

    [Header("Global Volume References")]
    [SerializeField] private Volume _volumePast;
    [SerializeField] private Volume _volumePresent;

    [Header("Transition Settings")]
    [SerializeField] private float _transitionDuration = 1.5f;

    private Coroutine _transitionCoroutine;

    private void Start()
    {
        SetVolumeInstant(GameManager.Instance.CurrentTemporality);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnTimeChangeStarted += BlendVolumes;
    }

    private void OnDisable()
    {
        if (GameManager.Instance)
            GameManager.Instance.OnTimeChangeStarted -= BlendVolumes;
    }

    private void BlendVolumes(Temporality temporality)
    {
        if (_transitionCoroutine != null)
            StopCoroutine(_transitionCoroutine);

        _transitionCoroutine = StartCoroutine(TransitionVolumes(temporality));
    }

    private IEnumerator TransitionVolumes(Temporality temporality)
    {
        float t = 0f;

        while (t < _transitionDuration)
        {
            float blend = t / _transitionDuration;

            if (_volumePast != null)
                _volumePast.weight = temporality == Temporality.Past ? blend : 1f - blend;

            if (_volumePresent != null)
                _volumePresent.weight = temporality == Temporality.Present ? 1f - blend : blend;

            t += Time.deltaTime;
            yield return null;
        }

        if (_volumePast != null)
            _volumePast.weight = temporality == Temporality.Past ? 1f : 0f;

        if (_volumePresent != null)
            _volumePresent.weight = temporality == Temporality.Present ? 0f : 1f;
    }

    private void SetVolumeInstant(Temporality temporality)
    {
        if (_volumePast != null)
            _volumePast.weight = temporality == Temporality.Past ? 1f : 0f;

        if (_volumePresent != null)
            _volumePresent.weight = temporality == Temporality.Past ? 0f : 1f;
    }
}
