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
    public DialogueUIType UIType;
    public DialogueSentence[] Sentences;
    public bool TriggerEvent;
    public DialogueEventType EventType;
}

[System.Serializable]
public struct DialogueSentence
{
    public SentenceTranslate TextTranslate;

    public DialogueOptions Options;

    public float SpeedWriting;
    public float TimeToPass;
    public WaitDialogueEventType WaitEventType;
}

[System.Flags]
public enum DialogueOptions
{
    None = 0,
    UseWriting = 1 << 0,
    DisableDialogueInputs = 1 << 1,
    UseTime = 1 << 2,
    WaitEvent = 1 << 3
}

public enum DialogueEventType
{
    None,
    RiwaSensaDamierDiscussionRoom3,
    RiwaShowDamierPath,
    RiwaEndShowingPath,
    ShowLianaPath,
    RiwaSensaLianaDiscussionRoom3,
    RiwaSensaDiscussRoom4,
    CameraStartFloor1Room2,
    RiwaOutFloor1Room2,
    RiwaInFloor1Room2,
    TriggerSequencerEvent,
    LookAtTreeRoom1,
    ResetCamRoom1,
    EnableChangeTime,
    AntreRiwaEntry,
    AntreRiwaChangeTempo,
    AntreRiwaCinematicEnd,
    PusleChangeTime,
    ShowInput,
    OnFinish,
    UnlockChangeTime,
    SensaSpeaking,
    EnableStumpRoom3,
    EnablePlantRoom2,
    Room0CollectibleDialogueEnd,
    Room0CollectibleInformationEnd,
    Room0CollectibleCamera,
    Room0MoveToCollectible,
    DisplayJoystick
}

public enum WaitDialogueEventType
{
    None,
    Test,
    SequenceCinematicFloor1Room2,
    SocleFloor1Room2,
    LianaFloor1Room2,
    RiwaHiddingIntoSensa,
    WaitEndOfLianaPathTravel,
    WaitForSensaLandingAtFragment,
    WaitPlayerToInteract,
    WaitShowRoom4End,
    ChangeTime,
    SensaFinishToSpeak,
    Move
}
