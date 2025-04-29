using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


[CreateAssetMenu(fileName = "Change Scene Modular Sequence", menuName = "Riwa/GenericAction/Change Scene Modular")]
public class SequenceActionChangeSceneModular : SequencerAction
{
    private Door _attachedDoor;

    public override void Initialize(GameObject obj)
    {
        _attachedDoor = obj.GetComponent<Door>();
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _attachedDoor = context.gameObject.GetComponent<Door>();
        _attachedDoor.ChangeScene();

        DialogueSystem.Instance.FinishDialogue();

        yield return null;
    }
}
