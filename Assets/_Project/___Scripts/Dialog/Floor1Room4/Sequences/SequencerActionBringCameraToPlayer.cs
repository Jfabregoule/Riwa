using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bring Camera To Player", menuName = "Riwa/Dialogue/Floor1/Room4/Sequences/Bring Camera To Player")]
public class SequencerActionBringCameraToPlayer : SequencerAction
{
    public Action ShowRoom4End;
    private Floor1Room4LevelManager _instance;
    private DialogueSystem _dialogueSystem;

    public override void Initialize(GameObject obj)
    {
        _instance = (Floor1Room4LevelManager)Floor1Room4LevelManager.Instance;
        _dialogueSystem = DialogueSystem.Instance;
        _dialogueSystem.EventRegistery.Register(WaitDialogueEventType.WaitShowRoom4End, ShowRoom4End);
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        for(int i = _instance.FresqueCameras.Count - 1; i >=0; i--)
        {
            for(int j = 0; j < _instance.FresqueCameras.Count; j++)
            {
                _instance.FresqueCameras[j].Priority = (j == i) ? 20 : 0;
            }

            yield return new WaitForSeconds(2f);
        }

        _instance.FresqueCameras[0].Priority = 0;
        _dialogueSystem.EventRegistery.Invoke(WaitDialogueEventType.WaitShowRoom4End);
    }
}
