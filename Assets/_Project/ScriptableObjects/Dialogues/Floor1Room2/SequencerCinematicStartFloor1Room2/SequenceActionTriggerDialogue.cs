using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trigger Dialogue", menuName = "Riwa/Dialogue/Floor1/Room2/Sequences/Trigger Dialogue")]
public class SequenceActionTriggerDialogue : SequencerAction
{
    public Action SequenceEnd;
    private DialogueSystem _dialogueSystem;
    public override void Initialize(GameObject obj)
    {
        _dialogueSystem = DialogueSystem.Instance;
        _dialogueSystem.EventRegistery.Register(WaitDialogueEventType.SequenceCinematicFloor1Room2, SequenceEnd);
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _dialogueSystem.EventRegistery.Invoke(WaitDialogueEventType.SequenceCinematicFloor1Room2);
        yield return null;
    }
}
