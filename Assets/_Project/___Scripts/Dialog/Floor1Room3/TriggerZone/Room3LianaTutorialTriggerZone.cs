using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room3LianaTutorialCollider : Room3DialogTriggerZoneParent
{

    private bool _hasBeenAlreadyTriggered = false;

    public override void DialogueToCall(EnumTemporality temporality)
    {
        if (_isPlayerInArea && temporality == EnumTemporality.Present && _hasBeenAlreadyTriggered == false)
        {
            Debug.Log("LianaTutorialCollider");
            _hasBeenAlreadyTriggered = true;
            DialogueSystem.Instance.BeginDialogue(_instance.TutorialRoom3Manager.LianaDialogue);
        }
    }
}
