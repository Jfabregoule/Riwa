using UnityEngine;

public class UIElementComponent : MonoBehaviour
{
    private PulseEffect _pulseEffect;
    private HighlightEffect _highlightEffect;
    private CanvasGroup _canvasGroup;

    public bool IsPulse;
    public bool IsHighlight;
    public bool IsShow;

    void Start()
    {
        TryGetComponent(out _pulseEffect);
        TryGetComponent(out _highlightEffect);
        TryGetComponent(out _canvasGroup);
    }

    public void StartPulsing()
    {
        _pulseEffect.StartPulsing();
    }

    public void StopPulsing()
    {
        _pulseEffect.StopPulsing();
    }

    public void StartHighlight()
    {
        _highlightEffect.StartHighlight();
    }

    public void StopHighlight()
    {
        _highlightEffect.StopHighlight();
    }

    public void Display()
    {
        Helpers.EnabledCanvasGroup(_canvasGroup);
    }

    public void Hide()
    {
        Helpers.DisabledCanvasGroup(_canvasGroup);
    }
}
