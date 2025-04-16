using System.Collections;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "End Dialogue", menuName = "Riwa/Dialogue/Sequences/End Dialogue")]
public class SequencerActionEndDialogue : SequencerAction
{
    private DialogueSystem _dialogueSystem;
    private CanvasGroup _fadeCanvas;
    private TextMeshProUGUI _dialogueText;

    public override void Initialize(GameObject obj)
    {
        _dialogueSystem = DialogueSystem.Instance;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _dialogueSystem.Reset();
        _dialogueSystem.EndDialogue();
        yield return null;
    }
}
