using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbeLight_TempoToggle : MonoBehaviour
{
    [SerializeField] private bool _isPast;

    [Header("Probe Light Datas")]
    [SerializeField] private LightProbeData _pastLightData;
    [SerializeField] private LightProbeData _presentLightData;

    private ChangeTime _changeTime;

    private void Start()
    {
        _changeTime = GameManager.Instance.Character.GetComponent<ChangeTime>();
        _changeTime.OnTimeChangeStarted += OnChangedTime;

        SetProbeSetInstant(_isPast);
    }

    private void OnChangedTime(bool isNowPast)
    {
        SetProbeSetInstant(isNowPast);
    }

    private void SetProbeSetInstant(bool isPast)
    {
        var data = isPast ? _presentLightData : _pastLightData;

        if (data == null || data.bakedProbes == null)
        {
            Debug.LogWarning("Invalid Light Probe Data");
            return;
        }

        LightmapSettings.lightProbes.bakedProbes = data.bakedProbes;
    }

    private void OnDestroy()
    {
        if (_changeTime != null)
            _changeTime.OnTimeChangeStarted -= OnChangedTime;
    }
}
