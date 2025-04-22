using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TranslateText : MonoBehaviour
{
    [SerializeField] private SentenceTranslate _sentence;

    private TextMeshProUGUI _translateText;

    private void Awake()
    {
        _translateText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameManager.Instance.TranslateSystem.OnLanguageChanged += SetText;
    }

    private void SetText()
    {
        if (_translateText == null)
        {
            Debug.Log(gameObject.name);
        }
        _translateText.text = _sentence.GetText(GameManager.Instance.TranslateSystem.GetCurrentLanguage());
    }

    public void SetSentenceTranslate(SentenceTranslate sentence) { 
        _sentence = sentence;
        SetText();
    }

    public SentenceTranslate GetSentenceTranslate()
    {
        return _sentence;
    }
}
