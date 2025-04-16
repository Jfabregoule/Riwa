using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fade In/Out Sequence", menuName = "Riwa/Dialogue/Sequences/Fade InOut")]
public class SequencerActionFadeInOut : SequencerAction
{
    [SerializeField] private float _fadeDuration = .6f;
    [SerializeField] private float _from = 0f;
    [SerializeField] private float _to = 1f;

    private DialogueSystem _dialogueSystem;

    public override void Initialize(GameObject obj)
    {
        _dialogueSystem = DialogueSystem.Instance;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        float elapsedTime = 0f;
        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            _dialogueSystem.ChangeCanvasGroupAlpha(Mathf.Lerp(_from, _to, elapsedTime / _fadeDuration));
            yield return null;
        }

        _dialogueSystem.ChangeCanvasGroupAlpha(_to);
    }
}
