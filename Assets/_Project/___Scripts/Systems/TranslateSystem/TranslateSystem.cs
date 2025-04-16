using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateSystem : MonoBehaviour
{
    public delegate void LanguageEvent();
    public event LanguageEvent OnLanguageChanged;

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
