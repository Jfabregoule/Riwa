using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiwaShowingPathTriggerZone : Room3DialogTriggerZoneParent
{
    public override void DialogueToCall(EnumTemporality temporality)
    {
        if (_isPlayerInArea && temporality == EnumTemporality.Present)
        {
            Debug.Log("RiwaShowPathTriggerZone");
            DialogueSystem.Instance.BeginDialogue(_instance.TutorialRoom3Manager.Room3Dialogue[2]);
        }
    }
}
