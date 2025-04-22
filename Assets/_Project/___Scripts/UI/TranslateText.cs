using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TranslateText : MonoBehaviour
{
    [SerializeField] private SentenceTranslate _sentence;

    private TextMeshProUGUI _translateText;
    void Start()
    {
        GameManager.Instance.TranslateSystem.OnLanguageChanged += SetText;
        _translateText = GetComponent<TextMeshProUGUI>();
        SetText();
    }

    private void SetText()
    {
        _translateText.text = _sentence.GetText(GameManager.Instance.TranslateSystem.GetCurrentLanguage());
    }

    public void SetSentenceTranslate(SentenceTranslate sentence) { 
        _sentence = sentence;
        SetText();
    }
}
