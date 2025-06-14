using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbeLight_TempoToggle : MonoBehaviour
{
    [SerializeField] private bool _isPast;

    [Header("Probe Light Datas")]
    [SerializeField] private LightProbeData _pastLightData;
    [SerializeField] private LightProbeData _presentLightData;

    private void Start()
    {
        SetProbeSetInstant(GameManager.Instance.CurrentTemporality);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnTimeChangeStarted += SetProbeSetInstant;
    }

    private void OnDisable()
    {
        if (GameManager.Instance)
            GameManager.Instance.OnTimeChangeStarted -= SetProbeSetInstant;
    }

    private void SetProbeSetInstant(EnumTemporality temporality)
    {
        LightProbeData data = temporality == EnumTemporality.Present ? _presentLightData : _pastLightData;

        if (data == null || data.bakedProbes == null)
        {
            Debug.LogWarning("Invalid Light Probe Data");
            return;
        }

        if (LightmapSettings.lightProbes)
            LightmapSettings.lightProbes.bakedProbes = data.bakedProbes;
    }
}
