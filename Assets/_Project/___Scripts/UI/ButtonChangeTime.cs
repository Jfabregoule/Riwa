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
        GameManager.Instance.OnIndicePulse += _pulseEffect.StartPulsing;
        GameManager.Instance.OnTimeChangeStarted += _pulseEffect.StopPulsing;
        GameManager.Instance.OnUnlockChangeTime += Display;
    }

    private void Display()
    {
        if (_control.IsRightHanded == _isRight)
        {
            Helpers.EnabledCanvasGroup(_canvasGroup);
        } 
    }
}
