using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Switch To Heart Camera", menuName = "Riwa/Room1/Switch To Heart Camera")]
public class SequenceActionSwitchToHeartCam : SequencerAction
{

    private Floor1Room1LevelManager _instance;

    public override void Initialize(GameObject obj)
    {
        _instance = (Floor1Room1LevelManager)Floor1Room1LevelManager.Instance;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _instance.CameraDictionnary[EnumCameraRoom.LastCameraRoom1].Priority = 30;
        yield return null;
    }
}
