using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Update Sentence", menuName = "Riwa/Dialogue/Sequences/Update Sentence")]
public class SequenceActionUpdateSentence : SequencerAction
{
    private DialogueSystem _dialogueSystem;

    public override void Initialize(GameObject obj)
    {
        _dialogueSystem = DialogueSystem.Instance;
    }
    public override IEnumerator StartSequence(Sequencer context)
    {
        _dialogueSystem.UpdateSentence();
        yield return null;
    }
}
