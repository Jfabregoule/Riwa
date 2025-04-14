using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Display First Sentence", menuName = "Malatre/Dialogue/Sequences/Display First Sentence")]
public class SequencerActionDisplayFirstSentence : SequencerAction
{
    private DialogueSystem _dialogueSystem;

    public override void Initialize(GameObject obj)
    {
        _dialogueSystem = DialogueSystem.Instance;
    }
    public override IEnumerator StartSequence(Sequencer context)
    {
        _dialogueSystem.UpdateSentence();
        InputManager.Instance.EnableDialogueControls();
        yield return null;
    }
}
