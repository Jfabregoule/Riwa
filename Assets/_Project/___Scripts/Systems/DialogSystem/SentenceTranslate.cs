using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Sentence
{
    public EnumLanguage Language;
    [TextArea] public string Text;
}

[CreateAssetMenu(fileName = "New Sentence Translate", menuName = "Riwa/Sentence")]
public class SentenceTranslate : ScriptableObject
{
    public List<Sentence> Sentences;
}
