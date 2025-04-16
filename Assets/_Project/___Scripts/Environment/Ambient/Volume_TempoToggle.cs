using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Volume_TempoToggle : MonoBehaviour
{
    [SerializeField] private bool _isPast;

    [Header("Global Volume References")]
    [SerializeField] private Volume _volumePast;
    [SerializeField] private Volume _volumeFuture;

    [Header("Transition Settings")]
    [SerializeField] private float _transitionDuration = 1.5f;

    private ChangeTime _changeTime;
    private Coroutine _transitionCoroutine;

    private void Start()
    {
        _changeTime = GameManager.Instance.Character.GetComponent<ChangeTime>();
        _changeTime.OnTimeChangeStarted += OnChangedTime;

        SetVolumeInstant(_isPast);
    }

    private void OnChangedTime(bool isNowPast)
    {
        if (_transitionCoroutine != null)
            StopCoroutine(_transitionCoroutine);

        _transitionCoroutine = StartCoroutine(TransitionVolumes(!isNowPast));
    }

    private IEnumerator TransitionVolumes(bool toPast)
    {
        float t = 0f;

        while (t < _transitionDuration)
        {
            float blend = t / _transitionDuration;

            if (_volumePast != null)
                _volumePast.weight = toPast ? blend : 1f - blend;

            if (_volumeFuture != null)
                _volumeFuture.weight = toPast ? 1f - blend : blend;

            t += Time.deltaTime;
            yield return null;
        }

        if (_volumePast != null)
            _volumePast.weight = toPast ? 1f : 0f;

        if (_volumeFuture != null)
            _volumeFuture.weight = toPast ? 0f : 1f;
    }

    private void SetVolumeInstant(bool isPast)
    {
        if (_volumePast != null)
            _volumePast.weight = isPast ? 1f : 0f;

        if (_volumeFuture != null)
            _volumeFuture.weight = isPast ? 0f : 1f;
    }

    private void OnDestroy()
    {
        if (_changeTime != null)
            _changeTime.OnTimeChangeEnd -= OnChangedTime;
    }
}
