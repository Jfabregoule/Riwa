using UnityEngine;

public class UIElementComponent : MonoBehaviour
{
    private PulseEffect _pulseEffect;
    private HighlightEffect _highlightEffect;
    void Start()
    {
        TryGetComponent(out _pulseEffect);
        TryGetComponent(out _highlightEffect);
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


}
