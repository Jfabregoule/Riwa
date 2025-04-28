using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnterFromRoom0", menuName = "Riwa/Room1/EnterFromRoom0")]
public class SequenceActionEnterFromRoom0 : SequencerAction
{
    private Floor1Room1LevelManager _levelManager;

    public override void Initialize(GameObject obj)
    {
        _levelManager = (Floor1Room1LevelManager)Floor1Room1LevelManager.Instance;
    }
    public override IEnumerator StartSequence(Sequencer context)
    {
        _levelManager.EnterFromRoom0();
        yield return null;
    }
}
