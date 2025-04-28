using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Reset Door Sequence", menuName = "Riwa/GenericAction/ResetDoor")]
public class SequencerActionResetDoor : SequencerAction
{
    private Door _door;

    public override void Initialize(GameObject obj)
    {
    }


    public override IEnumerator StartSequence(Sequencer context)
    {
        _door = context.GetComponent<Door>();
        _door?.ResetDoor();

        yield return null;
    }
}
