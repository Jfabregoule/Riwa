using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


[CreateAssetMenu(fileName = "Change Scene Sequence", menuName = "Riwa/GenericAction/Change Scene")]
public class SequenceActionChangeScene : SequencerAction
{
    [SerializeField] private int _floorNumber = 1;
    [SerializeField] private int _roomNumber = 1;
    [SerializeField] private DoorDirection _nextDoorDirection;
    [SerializeField] private int _nextDoorID = 0;

    public override void Initialize(GameObject obj)
    {
        
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        RiwaLoadSceneSystem.Instance.GoToNewScene(_floorNumber, _roomNumber, _nextDoorID, _nextDoorDirection);

        yield return null;
    }
}
