using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChangeTime : MonoBehaviour
{
    private PulseEffect _pulseEffect;
    private CanvasGroup _canvasGroup;
    [SerializeField] private bool _isRight;
    [SerializeField] private Control _control;

    private void Start()
    {
        _pulseEffect = GetComponent<PulseEffect>();
        _canvasGroup = GetComponent<CanvasGroup>();
        GameManager.Instance.OnResetSave += Reset;
        GameManager.Instance.OnChangeTimePulse += _pulseEffect.StartPulsing;
        GameManager.Instance.OnChangeTimeStopPulse += _pulseEffect.StopPulsing;
        GameManager.Instance.OnTimeChangeStarted += OnChangeTime;
        GameManager.Instance.OnUnlockChangeTime += Display;
    }

    private void OnDisable()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.OnChangeTimePulse -= _pulseEffect.StartPulsing;
            GameManager.Instance.OnChangeTimeStopPulse -= _pulseEffect.StopPulsing;
            GameManager.Instance.OnTimeChangeStarted -= OnChangeTime;
            GameManager.Instance.OnUnlockChangeTime -= Display;
            GameManager.Instance.OnResetSave -= Reset;
        }
    }

    private void Display()
    {
        if (_control.IsRightHanded == _isRight)
        {
            Helpers.EnabledCanvasGroup(_canvasGroup);
        } 
    }

    private void OnChangeTime(EnumTemporality temporality)
    {
        _pulseEffect.StopPulsing();
    }

    private void Reset() 
    {
        Helpers.DisabledCanvasGroup(_canvasGroup);
    }
}
