using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SequencerAction : ScriptableObject
{
    public abstract IEnumerator StartSequence(Sequencer context);

    public virtual void Initialize(GameObject obj) { }
}
