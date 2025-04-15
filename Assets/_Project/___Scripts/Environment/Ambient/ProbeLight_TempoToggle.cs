using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbeLight_TempoToggle : MonoBehaviour
{
    [SerializeField] private bool _isPast;

    [Header("Reflection Probe Sets")]
    [SerializeField] private List<ReflectionProbe> probeSetPast;
    [SerializeField] private List<ReflectionProbe> probeSetFuture;

    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 1.5f;

    private ChangeTime _changeTime;
    private Coroutine _transitionCoroutine;

    private void Start()
    {
        _changeTime = GameManager.Instance.Character.GetComponent<ChangeTime>();
        _changeTime.OnTimeChangeEnd += OnChangedTime;

        SetProbeSetInstant(_isPast);
    }

    private void OnChangedTime(bool isNowPast)
    {
        if (_transitionCoroutine != null)
            StopCoroutine(_transitionCoroutine);

        _transitionCoroutine = StartCoroutine(TransitionProbes(isNowPast));
    }

    private IEnumerator TransitionProbes(bool toPast)
    {
        List<ReflectionProbe> fromSet = toPast ? probeSetFuture : probeSetPast;
        List<ReflectionProbe> toSet = toPast ? probeSetPast : probeSetFuture;

        float t = 0f;

        while (t < transitionDuration)
        {
            float blend = t / transitionDuration;

            foreach (var probe in fromSet)
                if (probe != null) probe.intensity = 1f - blend;

            foreach (var probe in toSet)
                if (probe != null) probe.intensity = blend;

            t += Time.deltaTime;
            yield return null;
        }

        foreach (var probe in fromSet)
            if (probe != null) probe.intensity = 0f;

        foreach (var probe in toSet)
            if (probe != null) probe.intensity = 1f;
    }

    private void SetProbeSetInstant(bool isPast)
    {
        foreach (var probe in probeSetPast)
            if (probe != null) probe.intensity = isPast ? 1f : 0f;

        foreach (var probe in probeSetFuture)
            if (probe != null) probe.intensity = isPast ? 0f : 1f;
    }

    private void OnDestroy()
    {
        if (_changeTime != null)
            _changeTime.OnTimeChangeEnd -= OnChangedTime;
    }
}
