using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChangeTime : MonoBehaviour, IPulsable
{
    [SerializeField] private bool _isRight;

    private PulseEffect _pulseEffect;
    private CanvasGroup _canvasGroup;
    private Control _control;

    private void Start()
    {
        _pulseEffect = GetComponent<PulseEffect>();
        _canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(Helpers.WaitMonoBeheviour(() => GameManager.Instance.UIManager, WaitUIManager));
        GameManager.Instance.OnResetSave += Reset;
        GameManager.Instance.OnTimeChangeStarted += OnChangeTime;
        GameManager.Instance.OnUnlockChangeTime += Display;
    }

    private void OnDisable()
    {
        if (GameManager.Instance)
        {
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

    public void StartPulsing()
    {
        _pulseEffect.StartPulsing();
    }

    public void StopPulsing()
    {
        _pulseEffect.StopPulsing();
    }

    private void WaitUIManager(UIManager manager)
    {
        if (manager != null)
        {
            _control = manager.Control;
        }
    }
}
