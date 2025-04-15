using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Display First Sentence", menuName = "Riwa/Dialogue/Sequences/Display First Sentence")]
public class SequencerActionDisplayFirstSentence : SequencerAction
{
    private DialogueSystem _dialogueSystem;

    public override void Initialize(GameObject obj)
    {
        _dialogueSystem = DialogueSystem.Instance;
    }
    public override IEnumerator StartSequence(Sequencer context)
    {
        InputManager.Instance.EnableDialogueControls();
        _dialogueSystem.StartSection();
        yield return null;
    }
}
