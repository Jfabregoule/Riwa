using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Change UI Sequence", menuName = "Riwa/Dialogue/Sequences/Change UI")]

public class SequencerActionChangeUI : SequencerAction
{
    private DialogueSystem _dialogueSystem;
    public override void Initialize(GameObject obj)
    {
        _dialogueSystem = DialogueSystem.Instance;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _dialogueSystem.ChangeUI();
        yield return null;
    }
}
