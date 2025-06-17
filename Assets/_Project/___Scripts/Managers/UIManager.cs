using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public enum UIElementEnum
{
    ChangeTime, 
    Interact,
    Joystick,
    Pull,
    Push,
    Rotate
}

[System.Serializable]
public struct UIElement
{
    public UIElementEnum Enum;
    public bool IsRight;
    public UIElementComponent Element;
}

[DefaultExecutionOrder(-1)]
public class UIManager : MonoBehaviour
{
    [SerializeField] private Control _control;
    [SerializeField] private Navbar _navbar;
    [SerializeField] private BlackScreen _blackScreen;
    [SerializeField] private DialogueUIDispacher _dialogueUIDispacher;
    [SerializeField] private UIElement[] _uiElementsList;

    private Dictionary<UIElementEnum, Dictionary<bool, UIElementComponent>> _uiElements;

    public bool IsRightHanded { get; private set; }
    public Control Control { get { return _control; } }
    public BlackScreen BlackScreen { get { return _blackScreen; } }
    public DialogueUIDispacher DialogueUIDispacher { get { return _dialogueUIDispacher; } }

    public Navbar Navbar { get { return _navbar; } } 

    private void Awake()
    {
        _uiElements = _uiElementsList
            .GroupBy(e => e.Enum)
            .ToDictionary(
                g => g.Key,
                g => g.ToDictionary(e => e.IsRight, e => e.Element)
        );

    }

    public void StartPulse(UIElementEnum uiElementEnum)
    {
        _uiElements[uiElementEnum][IsRightHanded].StartPulsing();
    }
    public void StopPulse(UIElementEnum uiElementEnum)
    {
        _uiElements[uiElementEnum][IsRightHanded].StopPulsing();
    }

    public void StartHighlight(UIElementEnum uiElementEnum)
    {
        _uiElements[uiElementEnum][IsRightHanded].StartHighlight();
    }

    public void StopHighlight(UIElementEnum uiElementEnum)
    {
        _uiElements[uiElementEnum][IsRightHanded].StopHighlight();
    }

    public void Display(UIElementEnum uiElementEnum)
    {
        _uiElements[uiElementEnum][IsRightHanded].Display();
    }

    public void Hide(UIElementEnum uiElementEnum)
    {
        _uiElements[uiElementEnum][IsRightHanded].Hide();
    }

    public void SetHanded(bool handed) => IsRightHanded = handed;
}
