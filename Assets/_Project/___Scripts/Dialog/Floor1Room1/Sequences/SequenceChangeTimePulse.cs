using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Change Time Pulse", menuName = "Riwa/Dialogue/Floor1/Room1/Sequences/Change Time Pulse")]
public class SequenceChangeTimePulse : SequencerAction
{
    public override IEnumerator StartSequence(Sequencer context)
    {
        GameManager.Instance.PulseIndice();
        yield return null;
    }
}
