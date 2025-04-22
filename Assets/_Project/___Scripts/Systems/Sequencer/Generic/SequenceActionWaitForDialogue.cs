using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wait For Dialogue", menuName = "Riwa/GenericAction/WaitForDialogues")]
public class SequenceActionWaitForDialogue : SequencerAction
{
    private bool _canContinue;
    public override void Initialize(GameObject obj)
    {
        _canContinue = false;
        DialogueSystem.Instance.OnDialogueEvent += EventDispatcher;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        while (_canContinue) { 
            yield return null;
        }

    }

    public void EventDispatcher(DialogueEventType eventType)
    {
        switch (eventType)
        {
            case DialogueEventType.TriggerSequencerEvent:
                _canContinue = true;
                DialogueSystem.Instance.OnDialogueEvent -= EventDispatcher;
                break;
        }
    }

}