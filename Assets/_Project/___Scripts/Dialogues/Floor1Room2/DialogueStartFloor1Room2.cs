using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStartFloor1Room2 : MonoBehaviour
{
    [SerializeField] private DialogueAsset _assetStartDialogue;
    [SerializeField] private DialogueAsset _assetChangeTimeDialogue;
    [SerializeField] private CinemachineVirtualCamera[] _cameras;
    [SerializeField] private float _waitOnCamera = 3f;
    private DialogueSystem _dialogueSystem;

    public Action ChangeTime;

    private void OnDisable()
    {
        if (_dialogueSystem != null)
            _dialogueSystem.OnDialogueEvent -= DispatchDialogueEvent;

        if (GameManager.Instance)
        {
            GameManager.Instance.CurrentLevelManager.OnLevelEnter -= StartDialogue;
            GameManager.Instance.OnTimeChangeStarted -= StartChangeTimeDialogue;
        }
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

            case DialogueEventType.CameraStartFloor1Room2:
                StartCoroutine(SwitchCamera());
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
        StartCoroutine(Helpers.WaitMonoBeheviour(() => DialogueSystem.Instance, SubscribeToDialogueSystem));
    }

    private void StartChangeTimeDialogue(EnumTemporality temporality)
    {
        if (temporality != EnumTemporality.Past) return;
        GameManager.Instance.OnTimeChangeStarted -= StartChangeTimeDialogue;
        _dialogueSystem.BeginDialogue(_assetChangeTimeDialogue);
    }

    private IEnumerator SwitchCamera()
    {
        _cameras[0].Priority = 20;
        yield return new WaitForSeconds(_waitOnCamera);
        for (int i = 0; i < _cameras.Length - 1; i++)
        {
            _cameras[i].Priority = 0;
            _cameras[i + 1].Priority = 20;
            yield return new WaitForSeconds(_waitOnCamera);
        }
        _cameras[_cameras.Length - 1].Priority = 0;
    }
}
