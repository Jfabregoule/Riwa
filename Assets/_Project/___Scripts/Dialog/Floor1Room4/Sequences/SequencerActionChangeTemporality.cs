using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Change Temporality", menuName = "Riwa/Dialogue/Floor1/Room4/Sequences/Change Temporality")]
public class SequencerActionChangeTemporality : SequencerAction
{
    public override IEnumerator StartSequence(Sequencer context)
    {
        GameManager.Instance.Character.TriggerChangeTempo();
        yield return null;
    }
}
