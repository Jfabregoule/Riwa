using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move Camera Forward", menuName = "Riwa/Dialogue/Floor1/Room2/Sequences/MoveCameraForward")]
public class SequenceActionMoveDollyCamera : SequencerAction
{
    [SerializeField] private bool _forward;
    private CameraCinematicRoom2 _cameraCinematicRoom2;

    public override void Initialize(GameObject obj) 
    {
        _cameraCinematicRoom2 = ((Floor1Room2LevelManager)Floor1Room2LevelManager.Instance).CameraCinematicRoom2;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        yield return _cameraCinematicRoom2.StartTravel(_forward);
    }
}
