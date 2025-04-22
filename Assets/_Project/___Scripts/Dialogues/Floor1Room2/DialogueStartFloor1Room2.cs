using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStartFloor1Room2 : MonoBehaviour
{
    [SerializeField] private DialogueAsset _asset;
    [SerializeField] private Sequencer _sequencerCinematic;
    private DialogueSystem _dialogueSystem;

    private void OnDisable()
    {
        if (_dialogueSystem != null)
            _dialogueSystem.OnDialogueEvent -= DispatchDialogueEvent;

        if(GameManager.Instance)
            GameManager.Instance.CurrentLevelManager.OnLevelEnter -= StartDialogueAndCinematic;
    }
    private void Start()
    {
        GameManager.Instance.CurrentLevelManager.OnLevelEnter += StartDialogueAndCinematic;
    }
    private void DispatchDialogueEvent(DialogueEventType dialogueEventType)
    {
        switch (dialogueEventType)
        {
            case DialogueEventType.CameraStartFloor1Room2:
                _sequencerCinematic.InitializeSequence();
                break;
        }
    }

    private void SubscribeToDialogueSystem(DialogueSystem script)
    {
        if (script != null)
        {
            _dialogueSystem = script;
            _dialogueSystem.OnDialogueEvent += DispatchDialogueEvent;
            _dialogueSystem.BeginDialogue(_asset);
        }
    }

    private void StartDialogueAndCinematic()
    {
        _sequencerCinematic.Init();
        StartCoroutine(Helpers.WaitMonoBeheviour(() => DialogueSystem.Instance, SubscribeToDialogueSystem));
    }
}
