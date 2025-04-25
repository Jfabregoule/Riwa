using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicRoom0Manager : MonoBehaviour
{
    [SerializeField] private DialogueAsset _dialogueAsset;
    [SerializeField] private Sequencer _sequencerEntry;

    private Floor1Room0LevelManager _instance;

    private void Start()
    {
        _instance = (Floor1Room0LevelManager)Floor1Room0LevelManager.Instance;
        //_instance.OnLevelEnter += Init;
        _sequencerEntry.Init();
        DialogueSystem.Instance.OnDialogueEvent += DispatchEventOnDialogueEvent;
        DialogueSystem.Instance.BeginDialogue(_dialogueAsset);
    }

    //private void Init()
    //{
    //    Debug.Log("Player entry");
    //    _sequencerEntry.Init();
    //    //DialogueSystem.Instance.OnDialogueEvent += DispatchEventOnDialogueEvent;
    //    //DialogueSystem.Instance.BeginDialogue(_dialogueAsset);
    //}

    private void DispatchEventOnDialogueEvent(DialogueEventType dialogueEvent)
    {
        switch(dialogueEvent)
        {
            case DialogueEventType.AntreRiwaEntry:
                _sequencerEntry.InitializeSequence();
                break;
            case DialogueEventType.AntreRiwaChangeTempo:
                GameManager.Instance.Character.TriggerChangeTempo();
                break;
            case DialogueEventType.AntreRiwaCinematicEnd:
                _instance.IsCinematicDone = true;
                _instance.RiwaSensaCamera.Priority = 0;
                break;
        }
    }

}
