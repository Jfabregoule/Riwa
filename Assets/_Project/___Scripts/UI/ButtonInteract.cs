using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteract : MonoBehaviour
{
    private PulseEffect _pulseEffect;
    private void Start()
    {
        _pulseEffect = GetComponent<PulseEffect>();
        GameManager.Instance.OnInteractPulse += _pulseEffect.StartPulsing;
        GameManager.Instance.OnInteractStopPulse += _pulseEffect.StopPulsing;
        InputManager.Instance.OnInteract += _pulseEffect.StopPulsing;
    }

    private void OnDisable()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.OnInteractPulse -= _pulseEffect.StartPulsing;
            GameManager.Instance.OnInteractStopPulse -= _pulseEffect.StopPulsing;
            InputManager.Instance.OnInteract -= _pulseEffect.StopPulsing;
        }
    }
}
