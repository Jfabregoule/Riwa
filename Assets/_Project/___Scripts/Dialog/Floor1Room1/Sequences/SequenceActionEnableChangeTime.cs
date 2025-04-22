using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enable Change Time", menuName = "Riwa/Dialogue/Floor1/Room1/Sequences/Enable Change Time")]
public class SequenceActionEnableChangeTime : SequencerAction
{
    public override IEnumerator StartSequence(Sequencer context)
    {
        InputManager.Instance.EnableGameplayChangeTimeControls();
        yield return null;
    }
}
