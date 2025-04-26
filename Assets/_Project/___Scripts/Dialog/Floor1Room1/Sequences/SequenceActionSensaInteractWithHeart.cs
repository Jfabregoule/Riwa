using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Interact With Heart", menuName = "Riwa/Room1/Interact With Heart")]
public class SequenceActionSensaInteractWithHeart : SequencerAction
{
    public override IEnumerator StartSequence(Sequencer context)
    {
        GameManager.Instance.Character.Animator.SetTrigger("Interact");
        yield return null;
    }
}
