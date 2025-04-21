using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Change Camera Priority", menuName = "Riwa/Dialogue/Floor1/Room2/Sequences/Change Camera Priority")]
public class SequenceActionChangePriorityCamera : SequencerAction
{
    [SerializeField] private int _priority;
    private CinemachineVirtualCamera _cameraCinematicRoom2;
    public override void Initialize(GameObject obj)
    {
        _cameraCinematicRoom2 = ((Floor1Room2LevelManager)Floor1Room2LevelManager.Instance).CameraCinematicRoom2.GetComponent<CinemachineVirtualCamera>();
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _cameraCinematicRoom2.m_Priority = _priority;
        yield return null;
    }
}
