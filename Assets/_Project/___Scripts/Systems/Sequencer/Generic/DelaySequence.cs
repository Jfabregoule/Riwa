using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Delay Sequence", menuName = "Malatre/Sequences/Delay")]
public class DelaySequence : SequencerAction
{
    [SerializeField] private float _delay;

    public override IEnumerator StartSequence(Sequencer context)
    {
        yield return Helpers.GetWait(_delay);
    }
}
