using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Sentence
{
    public TranslateSystem.EnumLanguage Language;
    [TextArea] public string Text;
}

[CreateAssetMenu(fileName = "New Sentence Translate", menuName = "Riwa/Sentence")]
public class SentenceTranslate : ScriptableObject
{
    [SerializeField] private List<Sentence> Sentences;

    public string GetText(TranslateSystem.EnumLanguage language)
    {
        if (Sentences.Exists(it => it.Language == language))
        {
            return Sentences.Find(it => it.Language == language).Text;
        }

        return "";
    }
}
