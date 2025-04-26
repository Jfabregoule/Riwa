using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStartFloor1Room2 : MonoBehaviour
{
    [SerializeField] private DialogueAsset _assetStartDialogue;
    [SerializeField] private DialogueAsset _assetChangeTimeDialogue;
    //[SerializeField] private Sequencer _sequencerCinematic;
    private DialogueSystem _dialogueSystem;

    public Action ChangeTime;

    private void OnDisable()
    {
        if (_dialogueSystem != null)
            _dialogueSystem.OnDialogueEvent -= DispatchDialogueEvent;

        if(GameManager.Instance)
            GameManager.Instance.CurrentLevelManager.OnLevelEnter -= StartDialogue;
    }
    private void Start()
    {
        GameManager.Instance.CurrentLevelManager.OnLevelEnter += StartDialogue;
    }
    private void DispatchDialogueEvent(DialogueEventType dialogueEventType)
    {
        switch (dialogueEventType)
        {
            case DialogueEventType.PusleChangeTime:
                GameManager.Instance.PulseIndice();
                GameManager.Instance.OnTimeChangeStarted += StartChangeTimeDialogue;
                break;
        }
    }

    private void SubscribeToDialogueSystem(DialogueSystem script)
    {
        if (script != null)
        {
            _dialogueSystem = script;
            _dialogueSystem.OnDialogueEvent += DispatchDialogueEvent;
            _dialogueSystem.BeginDialogue(_assetStartDialogue);
        }
    }

    private void StartDialogue()
    {
        //_sequencerCinematic.Init();
        StartCoroutine(Helpers.WaitMonoBeheviour(() => DialogueSystem.Instance, SubscribeToDialogueSystem));
    }

    private void StartChangeTimeDialogue(EnumTemporality temporality)
    {
        if (temporality != EnumTemporality.Past) return;
        GameManager.Instance.OnTimeChangeStarted -= StartChangeTimeDialogue;
        _dialogueSystem.BeginDialogue(_assetChangeTimeDialogue);
    }
}
