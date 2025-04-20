using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateSystem : MonoBehaviour
{
    public delegate void LanguageEvent();
    public event LanguageEvent OnLanguageChanged;

    public Action test;

    private EnumLanguage CurrentLanguage;
    public enum EnumLanguage
    {
        French,
        Spanish,
        Italian,
        English,
        German,
        Japanese
    }

    private void Start()
    {
        //DialogueSystem.Instance.EventRegistery.Register(WaitDialogueEventType.Test, test);
        //DialogueSystem.Instance.RegisterWaitEvent(WaitDialogueEventType.Test, () => test?.Invoke());
    }
    public void ChangeLanguage(EnumLanguage language)
    {
        CurrentLanguage = language;
        OnLanguageChanged?.Invoke();
        //DialogueSystem.Instance.EventRegistery.Invoke(WaitDialogueEventType.Test);
    }

    public EnumLanguage GetCurrentLanguage()
    {
        return CurrentLanguage;
    }
}
