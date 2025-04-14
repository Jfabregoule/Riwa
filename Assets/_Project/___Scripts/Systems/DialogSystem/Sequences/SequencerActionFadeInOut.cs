using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fade In/Out Sequence", menuName = "Malatre/Dialogue/Sequences/Fade InOut")]
public class SequencerActionFadeInOut : SequencerAction
{
    [SerializeField] private float _fadeDuration = .6f;
    [SerializeField] private float _from = 0f;
    [SerializeField] private float _to = 1f;

    private CanvasGroup _fadeCanvas;

    public override void Initialize(GameObject obj)
    {
        _fadeCanvas = DialogueSystem.Instance.FadeCanvas;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _fadeCanvas.gameObject.SetActive(true);


        float elapsedTime = 0f;
        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            _fadeCanvas.alpha = Mathf.Lerp(_from, _to, (elapsedTime / _fadeDuration));
            yield return null;
        }

        _fadeCanvas.alpha = _to;
    }
}
