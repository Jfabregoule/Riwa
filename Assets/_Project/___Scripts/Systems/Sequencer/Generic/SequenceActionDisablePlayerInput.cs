using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Disable Player Input", menuName = "Riwa/GenericAction/Disable Player Input")]
public class SequenceActionDisablePlayerInput : SequencerAction
{
    public override IEnumerator StartSequence(Sequencer context)
    {
        InputManager.Instance.DisableGameplayControls();
        yield return null;
    }
}
