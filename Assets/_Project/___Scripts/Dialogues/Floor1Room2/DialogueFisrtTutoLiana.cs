using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueFisrtTutoLiana : MonoBehaviour
{
    [SerializeField] private DialogueAsset _asset;
    private DialogueSystem _dialogueSystem;
    private bool _isAlreadyTrigger = false;

    private void OnDisable()
    {
        if (_dialogueSystem != null)
            _dialogueSystem.OnDialogueEvent -= DispatchDialogueEvent;
    }
    private void Start()
    {
        StartCoroutine(Helpers.WaitMonoBeheviour(() => DialogueSystem.Instance, SubscribeToDialogueSystem));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out ACharacter character) || _isAlreadyTrigger) return;

        _isAlreadyTrigger = true;
        _dialogueSystem.BeginDialogue(_asset);
    }

    private void DispatchDialogueEvent(DialogueEventType dialogueEventType)
    {
        switch (dialogueEventType)
        {
            case DialogueEventType.RiwaOutFloor1Room2:
                ((Floor1Room2LevelManager)Floor1Room2LevelManager.Instance).RiwaFloor1Room2.MoveOutSensa();
                break;
            case DialogueEventType.RiwaInFloor1Room2:
                ((Floor1Room2LevelManager)Floor1Room2LevelManager.Instance).RiwaFloor1Room2.MoveToSensa();
                break;
            case DialogueEventType.EnablePlantRoom2:
                ((Floor1Room2LevelManager)Floor1Room2LevelManager.Instance).BridgeVineScript.CanInteract = true;
                break;
        }
    }

    private void SubscribeToDialogueSystem(DialogueSystem script)
    {
        if (script != null)
        {
            _dialogueSystem = script;
            _dialogueSystem.OnDialogueEvent += DispatchDialogueEvent;
        }
    }
}
