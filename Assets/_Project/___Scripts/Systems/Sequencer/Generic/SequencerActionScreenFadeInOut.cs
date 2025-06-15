using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Screen Fade InOut Sequence", menuName = "Riwa/GenericAction/ScreenFadeInOut")]
public class SequencerActionScreenFadeInOut : SequencerAction
{
    [SerializeField] private float _fadeSpeed = 1f;
    [SerializeField] private bool _fadeIn = true;

    private bool _isFading;
    private BlackScreen _blackScreen;

    public override void Initialize(GameObject obj)
    {
        _blackScreen = GameManager.Instance.UIManager.BlackScreen;
    }


    public override IEnumerator StartSequence(Sequencer context)
    {

        if (_fadeIn)
            _blackScreen.FadeIn(1, _fadeSpeed);
        else
            _blackScreen.FadeOut(_fadeSpeed);

        yield return new WaitForSeconds(_fadeSpeed);
    }

    public void Finish()
    {
        _isFading = false;
    }

}
