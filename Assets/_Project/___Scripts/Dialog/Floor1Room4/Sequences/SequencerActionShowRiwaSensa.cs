using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Show Sensa & Riwa", menuName = "Riwa/Dialogue/Floor1/Room4/Sequences/Show Sensa & Riwa")]
public class SequencerActionShowRiwaSensa : SequencerAction
{
    private Floor1Room4LevelManager _instance;
    public bool ShowRiwaSensa;

    public override void Initialize(GameObject obj)
    {
        _instance = (Floor1Room4LevelManager)Floor1Room4LevelManager.Instance;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        if (ShowRiwaSensa) _instance.SensaRiwaDiscussing.Priority = 20;
        else _instance.SensaRiwaDiscussing.Priority = 0;
        yield return null;
    }
}
