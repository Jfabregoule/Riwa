using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
struct LanguageButton
{
    public Button button;
    public Image image;
}

public class LanguageSelector : MonoBehaviour
{
    [SerializeField] private LanguageButton[] _buttonLists;

    private TranslateSystem _translateSystem;

    private void Start()
    {
        _translateSystem = GameManager.Instance.TranslateSystem;
        for (int i = 0; i < _buttonLists.Length; i++)
        {
            TranslateSystem.EnumLanguage language = (TranslateSystem.EnumLanguage)i;
            AddListener(_buttonLists[i].button, language);
        }
        SelectLanguage(TranslateSystem.EnumLanguage.French);
    }

    private void AddListener(Button btn, TranslateSystem.EnumLanguage language)
    {
        btn.onClick.AddListener(() => SelectLanguage(language));
    }

    private void SelectLanguage(TranslateSystem.EnumLanguage language)
    {
        int index = (int)language;
        if (GetIntCurrentLanguage() != -1)
            _buttonLists[GetIntCurrentLanguage()].image.enabled = false;

        _buttonLists[index].image.enabled = true;
        _translateSystem.ChangeLanguage(language);
    }

    private int GetIntCurrentLanguage() => (int)_translateSystem.GetCurrentLanguage();
}
