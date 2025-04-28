using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SensaSpeak", menuName = "Riwa/Dialogue/Floor1/Room0/Sequences/SensaSpeak")]
public class SequenceActionSensaSpeak : SequencerAction
{
    private ACharacter character;
    private bool _canSkip;

    public override void Initialize(GameObject obj)
    {
        character = GameManager.Instance.Character;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _canSkip = false;

        character.OnFinishAnimationSpeak += SetTrueCanSkip;
        character.Animator.SetTrigger("CinematicSpeak");

        while (_canSkip)
        {
            yield return null;
        }

        character.OnFinishAnimationSpeak -= SetTrueCanSkip;

    }

    public void SetTrueCanSkip() {
        _canSkip = true;
    }

}
