using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enable Player Input", menuName = "Riwa/GenericAction/Enable Player Input")]
public class SequenceActionEnablePlayerInput : SequencerAction
{
    public override IEnumerator StartSequence(Sequencer context)
    {
        InputManager.Instance.EnableGameplayControls();
        yield return null;
    }
}
