using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Volume_TempoToggle : MonoBehaviour
{
    [SerializeField] private bool _isPast;

    [Header("Global Volume References")]
    [SerializeField] private Volume volumePast;
    [SerializeField] private Volume volumeFuture;

    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 1.5f;

    private ChangeTime _changeTime;
    private Coroutine _transitionCoroutine;

    private void Start()
    {
        _changeTime = GameManager.Instance.Character.GetComponent<ChangeTime>();
        _changeTime.OnTimeChangeEnd += OnChangedTime;

        SetVolumeInstant(_isPast);
    }

    private void OnChangedTime(bool isNowPast)
    {
        if (_transitionCoroutine != null)
            StopCoroutine(_transitionCoroutine);

        _transitionCoroutine = StartCoroutine(TransitionVolumes(isNowPast));
    }

    private IEnumerator TransitionVolumes(bool toPast)
    {
        float t = 0f;

        while (t < transitionDuration)
        {
            float blend = t / transitionDuration;

            if (volumePast != null)
                volumePast.weight = toPast ? blend : 1f - blend;

            if (volumeFuture != null)
                volumeFuture.weight = toPast ? 1f - blend : blend;

            t += Time.deltaTime;
            yield return null;
        }

        if (volumePast != null)
            volumePast.weight = toPast ? 1f : 0f;

        if (volumeFuture != null)
            volumeFuture.weight = toPast ? 0f : 1f;
    }

    private void SetVolumeInstant(bool isPast)
    {
        if (volumePast != null)
            volumePast.weight = isPast ? 1f : 0f;

        if (volumeFuture != null)
            volumeFuture.weight = isPast ? 0f : 1f;
    }

    private void OnDestroy()
    {
        if (_changeTime != null)
            _changeTime.OnTimeChangeEnd -= OnChangedTime;
    }
}
