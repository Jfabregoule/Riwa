using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Change Tempo", menuName = "Riwa/Dialogue/Floor1/Room2/Sequences/Change Tempo")]
public class SequenceActionChangeTemporality : SequencerAction
{
    public override void Initialize(GameObject obj)
    {
        
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        GameManager.Instance.Character.TriggerChangeTempo();
        yield return null;
    }
}
