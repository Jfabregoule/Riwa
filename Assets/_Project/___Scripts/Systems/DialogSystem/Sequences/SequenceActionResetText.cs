using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reset Text", menuName = "Riwa/Dialogue/Sequences/Reset Text")]
public class SequenceActionResetText : SequencerAction
{
    private DialogueSystem _dialogueSystem;

    public override void Initialize(GameObject obj)
    {
        _dialogueSystem = DialogueSystem.Instance;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _dialogueSystem.ResetText();
        yield return null;
    }
}
