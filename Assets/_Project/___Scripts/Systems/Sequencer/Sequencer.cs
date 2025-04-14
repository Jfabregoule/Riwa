using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequencer : MonoBehaviour
{
    public List<SequencerAction> SequenceActions;

    public void Init()
    {
        foreach (SequencerAction action in SequenceActions)
        {
            action.Initialize(gameObject);
        }
    }

    public void InitializeSequence()
    {
        StartCoroutine(ExecuteSequence());
    }

    private IEnumerator ExecuteSequence()
    {
        foreach (SequencerAction action in SequenceActions)
        {
            yield return StartCoroutine(action.StartSequence(this));
        }
    }
}
