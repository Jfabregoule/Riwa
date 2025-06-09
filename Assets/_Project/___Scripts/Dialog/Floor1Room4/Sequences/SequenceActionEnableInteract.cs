using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enable Interact", menuName = "Riwa/Dialogue/Floor1/Room4/Sequences/Enable Interact")]
public class SequenceActionEnableInteract : SequencerAction
{
    public override IEnumerator StartSequence(Sequencer context)
    {
        InputManager.Instance.EnableGameplayInteractControls();
        GameManager.Instance.UIManager.StartPulse(UIPulseEnum.Interact);
        yield return null;
    }
}
