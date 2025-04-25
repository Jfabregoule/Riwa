using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Switch Camera", menuName = "Riwa/Dialogue/Floor1/Room0/Sequences/Switch Camera")]
public class SequenceActionSwitchCamera : SequencerAction
{
    public bool ShowRiwaAndSensa = true;
    private Floor1Room0LevelManager _instance;

    public override void Initialize(GameObject obj)
    {
        _instance = (Floor1Room0LevelManager)Floor1Room0LevelManager.Instance;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        if(ShowRiwaAndSensa == true) _instance.RiwaSensaCamera.Priority = 110;
        else _instance.RiwaSensaCamera.Priority = 0;
        yield return null;
    }
}
