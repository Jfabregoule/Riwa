using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "EnableBacktracking", menuName = "Riwa/Room1/EnableBacktracking")]
public class SequenceActionEnableBacktracking : SequencerAction
{
    private Floor1Room1LevelManager _levelManager;

    public override void Initialize(GameObject obj)
    {
        _levelManager = (Floor1Room1LevelManager)Floor1Room1LevelManager.Instance;
    }
    public override IEnumerator StartSequence(Sequencer context)
    {
        _levelManager.EnableBacktracking();
        yield return null;
    }
}
