using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reset Controls", menuName = "Malatre/Dialogue/Sequences/Reset Controls")]
public class SequencerActionResetControls : SequencerAction
{
    private DialogueSystem _dialogueSystem;

    public override void Initialize(GameObject obj)
    {
        _dialogueSystem = DialogueSystem.Instance;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        InputManager.Instance.DisableDialogueControls();

        if (_dialogueSystem.ProcessingDialogue.EnablePlayerInputsOnClosure)
        {
            InputManager.Instance?.EnableGameplayControls();
            // Enable EventSystem
        }
        yield return null;
    }
}
