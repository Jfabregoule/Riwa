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
    public delegate void ChangeLanguage(int language);
    public ChangeLanguage OnChangeLanguage;

    [SerializeField] private LanguageButton[] _buttonLists;
    private Language _currentLanguage = Language.France;
    private enum Language : int
    {
        France,
        Espagnol,
        Italien,
        Anglais,
        Allemand,
        Japonais
    }

    private void Start()
    {
        for (int i = 0; i < _buttonLists.Length; i++)
        {
            Language language = (Language)i;
            AddListener(_buttonLists[i].button, language);
        }
        SelectLanguage(Language.France);
    }

    private void AddListener(Button btn, Language language)
    {
        btn.onClick.AddListener(() => SelectLanguage(language));
    }

    private void SelectLanguage(Language language)
    {
        int index = (int)language;
        if ((int)_currentLanguage!= -1)
            _buttonLists[(int)_currentLanguage].image.enabled = false;

        _buttonLists[index].image.enabled = true;
        _currentLanguage = language;
        OnChangeLanguage?.Invoke((int)_currentLanguage);
    }
}
