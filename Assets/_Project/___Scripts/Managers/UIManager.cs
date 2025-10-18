using System;
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
    Rotate,
    Moveable,
    Rotatable,
    ChangeTimeParent
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

    public bool RotationUnlocked { get; set; }

    private void Awake()
    {
        _uiElements = _uiElementsList
            .GroupBy(e => e.Enum)
            .ToDictionary(
                g => g.Key,
                g => g.ToDictionary(e => e.IsRight, e => e.Element)
        );

    }

    private void OnEnable()
    {
        LoadData();
        SaveSystem.Instance.OnLoadProgress += LoadData;
    }

    private void LoadData()
    {
        RotationUnlocked = SaveSystem.Instance.LoadElement<bool>("RotationUnlocked");
    }

    private void OnDisable()
    {
        SaveSystem.Instance.OnLoadProgress -= LoadData;
        SaveSystem.Instance.SaveElement<bool>("RotationUnlocked", RotationUnlocked);
    }

    private void Start()
    {
        //Display(UIElementEnum.Joystick);
        //Display(UIElementEnum.Interact);
        //Display(UIElementEnum.ChangeTime);
        //Display(UIElementEnum.Push);
        //if (RotationUnlocked == true)
        //    Display(UIElementEnum.Rotate);
    }

    public void StartPulse(UIElementEnum uiElementEnum)
    {
        _uiElements[uiElementEnum][IsRightHanded].StartPulsing();

        _uiElements[uiElementEnum][true].IsPulse = true;
        _uiElements[uiElementEnum][false].IsPulse = true;
    }
    public void StopPulse(UIElementEnum uiElementEnum)
    {
        _uiElements[uiElementEnum][IsRightHanded].StopPulsing();

        _uiElements[uiElementEnum][true].IsPulse = false;
        _uiElements[uiElementEnum][false].IsPulse = false;
    }

    public void StartHighlight(UIElementEnum uiElementEnum)
    {
        _uiElements[uiElementEnum][IsRightHanded].StartHighlight();

        _uiElements[uiElementEnum][true].IsHighlight = true;
        _uiElements[uiElementEnum][false].IsHighlight = true;
    }

    public void StopHighlight(UIElementEnum uiElementEnum)
    {
        _uiElements[uiElementEnum][IsRightHanded].StopHighlight();

        _uiElements[uiElementEnum][true].IsHighlight = false;
        _uiElements[uiElementEnum][false].IsHighlight = false;
    }

    public void Display(UIElementEnum uiElementEnum)
    {
        _uiElements[uiElementEnum][IsRightHanded].Display();

        _uiElements[uiElementEnum][true].IsShow = true;
        _uiElements[uiElementEnum][false].IsShow = true;
    }

    public void Hide(UIElementEnum uiElementEnum)
    {
        _uiElements[uiElementEnum][IsRightHanded].Hide();

        _uiElements[uiElementEnum][true].IsShow = false;
        _uiElements[uiElementEnum][false].IsShow = false;
    }

    public void SetHanded(bool handed) {
        IsRightHanded = handed;
        foreach(UIElement element in _uiElementsList)
        {
            if (element.Element.IsShow)
            {
                if(element.IsRight == IsRightHanded)
                {
                    element.Element.Display();
                }
                else
                {
                    element.Element.Hide();
                }
            }

            if(element.Element.IsHighlight)
            {
                if(element.IsRight == IsRightHanded)
                {
                    element.Element.StartHighlight();
                }
                else
                {
                    element.Element.StopHighlight();
                }
            }

            if (element.Element.IsPulse)
            {
                if(element.IsRight == IsRightHanded)
                {
                    element.Element.StartPulsing();
                }
                else
                {
                    element.Element.StopPulsing();
                }
            }
        }
    } 
}
