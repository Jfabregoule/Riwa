using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialogRoom4Manager : MonoBehaviour
{
    [Header("Sequencer")]
    [SerializeField] private List<Sequencer> _sequencerCinematics;

    private DialogueSystem _dialogueSystem;
    private Floor1Room4LevelManager _instance;

    public Action PlayerHasInteracted;

    private void Start()
    {
        _instance = (Floor1Room4LevelManager)Floor1Room4LevelManager.Instance;
        _instance.OnLevelEnter += Init;
    }

    private void Init()
    {
        if (_instance.IsTutorialDone == true) return;
        _instance.MuralPiece.OnPickUp += OnTutorialMuralPiecePickup;
        _sequencerCinematics[0].Init();
        _sequencerCinematics[1].Init();
        StartCoroutine(Helpers.WaitMonoBeheviour(() => DialogueSystem.Instance, SubscribeToDialogueSystem));
    }

    private void SubscribeToDialogueSystem(DialogueSystem dialogueSystem)
    {
        if(dialogueSystem != null)
        {
            _dialogueSystem = dialogueSystem;
            _dialogueSystem.EventRegistery.Register(WaitDialogueEventType.WaitPlayerToInteract, PlayerHasInteracted);
            _sequencerCinematics[0].InitializeSequence();
        }
    }

    private void OnTutorialMuralPiecePickup(MuralPiece piece)
    {
        _instance.IsTutorialDone = true;
        _dialogueSystem.EventRegistery.Invoke(WaitDialogueEventType.WaitPlayerToInteract);
        _sequencerCinematics[1].InitializeSequence();
    }
}
