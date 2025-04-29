using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[CreateAssetMenu(fileName = "Show Crate", menuName = "Riwa/Dialogue/Floor1/Room1/Sequences/Show Crate")]
public class SequenceActionShowCrate : SequencerAction
{
    private Floor1Room1LevelManager _instance;

    public override void Initialize(GameObject obj)
    {
        _instance = (Floor1Room1LevelManager)Floor1Room1LevelManager.Instance;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _instance.LianaCamera.Priority = 0;
        _instance.CrateCamera.Priority = 50;
        yield return null;
    }
}
