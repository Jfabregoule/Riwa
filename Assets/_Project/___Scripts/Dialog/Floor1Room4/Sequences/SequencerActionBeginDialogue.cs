using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Begin Dialogue", menuName = "Riwa/Dialogue/Floor1/Room4/Sequences/Begin Dialogue")]
public class SequencerActionBeginDialogue : SequencerAction
{
    [SerializeField] private DialogueAsset _dialogueAsset;
    private DialogueSystem _dialogueSystem;

    public override void Initialize(GameObject obj)
    {
        _dialogueSystem = DialogueSystem.Instance;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _dialogueSystem.BeginDialogue(_dialogueAsset);
        yield return null;
    }
}
