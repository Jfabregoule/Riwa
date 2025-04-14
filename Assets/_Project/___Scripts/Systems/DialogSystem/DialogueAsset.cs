using System.Collections;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Asset", menuName = "Malatre/Dialogue/Asset")]
public class DialogueAsset : ScriptableObject
{
    [Header("Settings")]
    public bool OpeningTriggerEvent;
    public DialogueEventType OpeningEventType;
    public bool ClosureTriggerEvent;
    public DialogueEventType ClosureEventType;
    public bool DisablePlayerInputsOnOpening = true;
    public bool EnablePlayerInputsOnClosure = true;
    [Space]
    public DialogueSection[] Sections;
}

[System.Serializable]
public struct DialogueSection
{
    [TextArea] public string[] Sentences;
    public bool DisableDialogueInputs;
    public bool TriggerEvent;
    public DialogueEventType EventType;
}

public enum DialogueEventType
{
    None,
}
