using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LaunchDialogueRoom1", menuName = "Riwa/Room1/LanchDialogue")]
public class SequenceActionLanchRoom1Dialogue : SequencerAction
{
    Floor1Room1Dialogue _dialogue;
    [SerializeField] int _dialogueIndex = 0;    

    public override void Initialize(GameObject obj)
    {
        Floor1Room1LevelManager manager = (Floor1Room1LevelManager)GameManager.Instance.CurrentLevelManager;
        _dialogue = manager.Dialogue;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _dialogue.LaunchDialogue(_dialogueIndex);

        yield return null;
    }
}
