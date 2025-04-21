using System.Collections;
using System.Collections.Generic;
public class RiwaShowingPathTriggerZone : Room3DialogTriggerZoneParent
{
    public override void DialogueToCall(EnumTemporality temporality)
    {
        if (_isPlayerInArea && temporality == EnumTemporality.Present)
            DialogueSystem.Instance.BeginDialogue(_instance.TutorialRoom3Manager.Room3Dialogue[2]);
    }
}
