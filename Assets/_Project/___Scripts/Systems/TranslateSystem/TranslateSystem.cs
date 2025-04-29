using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Icons;

public class TranslateSystem : MonoBehaviour
{
    public delegate void LanguageEvent();
    public event LanguageEvent OnLanguageChanged;

    private EnumLanguage CurrentLanguage = EnumLanguage.French;
    public enum EnumLanguage
    {
        French,
        Spanish,
        Italian,
        English,
        German,
        Portuguese
    }

    public void ChangeLanguage(EnumLanguage language)
    {
        CurrentLanguage = language;
        OnLanguageChanged?.Invoke();
    }

    public EnumLanguage GetCurrentLanguage()
    {
        return CurrentLanguage;
    }

}
