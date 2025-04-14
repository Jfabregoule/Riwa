using System.Collections;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "End Dialogue", menuName = "Malatre/Dialogue/Sequences/End Dialogue")]
public class SequencerActionEndDialogue : SequencerAction
{
    private DialogueSystem _dialogueSystem;
    private CanvasGroup _fadeCanvas;
    private TextMeshProUGUI _dialogueText;

    public override void Initialize(GameObject obj)
    {
        _dialogueSystem = DialogueSystem.Instance;
        _fadeCanvas = _dialogueSystem.FadeCanvas;
        _dialogueText = _dialogueSystem.DialogueText;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _fadeCanvas.gameObject.SetActive(false);
        _dialogueText.SetText("");
        _dialogueSystem.EndDialogue();
        yield return null;
    }
}
