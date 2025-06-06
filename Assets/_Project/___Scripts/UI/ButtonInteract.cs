using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteract : MonoBehaviour,IPulsable
{
    private PulseEffect _pulseEffect;
    private void Start()
    {
        _pulseEffect = GetComponent<PulseEffect>();

        InputManager.Instance.OnInteract += _pulseEffect.StopPulsing;
    }

    private void OnDisable()
    {
        if (GameManager.Instance)
        {
            InputManager.Instance.OnInteract -= _pulseEffect.StopPulsing;
        }
    }

    public void StartPulsing()
    {
        _pulseEffect.StartPulsing();
    }

    public void StopPulsing()
    {
        _pulseEffect.StopPulsing();
    }
}
