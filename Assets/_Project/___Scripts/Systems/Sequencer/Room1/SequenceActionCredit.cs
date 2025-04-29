using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Credit", menuName = "Riwa/Sequences/Generic/Credit")]
public class SequenceActionCredit : SequencerAction
{
    public override IEnumerator StartSequence(Sequencer context)
    {
        GameManager.Instance.InvokeCredit();
        yield return null;
    }
}
