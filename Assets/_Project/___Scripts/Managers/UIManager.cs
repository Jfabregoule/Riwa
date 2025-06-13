using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public enum UIElementEnum
{
    ChangeTime, 
    Interact
}

[System.Serializable]
public struct UIElement
{
    public UIElementEnum Enum;
    public UIElementComponent Element;
}

[DefaultExecutionOrder(-1)]
public class UIManager : MonoBehaviour
{
    [SerializeField] private Control _control;
    [SerializeField] private Navbar _navbar;
    [SerializeField] private BlackScreen _blackScreen;
    [SerializeField] private List<UIElement> _uiElementsList;

    private ReadOnlyDictionary<UIElementEnum, UIElementComponent> _uiElements;

    public bool IsRightHanded { get; private set; }
    public Control Control { get { return _control; } }
    public BlackScreen BlackScreen { get { return _blackScreen; } }
    
    public Navbar Navbar { get { return _navbar; } } 

    private void Awake()
    {
        _uiElements = new ReadOnlyDictionary<UIElementEnum, UIElementComponent>(
            _uiElementsList.ToDictionary(e => e.Enum, e => e.Element)
        );
    }

    public void StartPulse(UIElementEnum uiElementEnum)
    {
        _uiElements[uiElementEnum].StartPulsing();
    }
    public void StopPulse(UIElementEnum uiElementEnum)
    {
        _uiElements[uiElementEnum].StopPulsing();
    }

    public void StartHighlight(UIElementEnum uiElementEnum)
    {
        _uiElements[uiElementEnum].StartHighlight();
    }

    public void StopHighlight(UIElementEnum uiElementEnum)
    {
        _uiElements[uiElementEnum].StopHighlight();
    }

    public void SetHanded(bool handed) => IsRightHanded = handed;
}
