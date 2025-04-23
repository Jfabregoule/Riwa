using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChangeTime : MonoBehaviour
{
    private PulseEffect _pulseEffect;

    private void Start()
    {
        _pulseEffect = GetComponent<PulseEffect>();
        GameManager.Instance.OnIndicePulse += _pulseEffect.StartPulsing;
        GameManager.Instance.OnTimeChangeStarted += _pulseEffect.StopPulsing;
    }
}
