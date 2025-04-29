using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[CreateAssetMenu(fileName = "Show Liana", menuName = "Riwa/Dialogue/Floor1/Room1/Sequences/Show Liana")]
public class SequenceActionShowLiana : SequencerAction
{
    private Floor1Room1LevelManager _instance;

    public override void Initialize(GameObject obj)
    {
        _instance = (Floor1Room1LevelManager)Floor1Room1LevelManager.Instance;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _instance.CrateCamera.Priority = 0;
        _instance.LianaCamera.Priority = 50;
        yield return null;
    }
}
