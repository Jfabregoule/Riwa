using System.Collections;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Asset", menuName = "Riwa/Dialogue/Asset")]
public class DialogueAsset : ScriptableObject
{
    [Header("Settings")]
    public bool OpeningTriggerEvent;
    public DialogueEventType OpeningEventType;
    public bool ClosureTriggerEvent;
    public DialogueEventType ClosureEventType;
    public bool DisablePlayerInputsOnOpening = true;
    public bool EnablePlayerInputsOnClosure = true;
    public bool IsAllSkipable = false;
    [Space]
    public DialogueSection[] Sections;
}

[System.Serializable]
public struct DialogueSection
{
    public bool DisableDialogueInputs;
    public DialogueSentence[] Sentences;
    public bool TriggerEvent;
    public DialogueEventType EventType;
}

[System.Serializable]
public struct DialogueSentence
{
    public SentenceTranslate TextTranslate;
    public bool UseWriting;
    public float SpeedWriting;
    public bool UseTime;
    public float TimeToPass;
}

public enum DialogueEventType
{
    None,
}
