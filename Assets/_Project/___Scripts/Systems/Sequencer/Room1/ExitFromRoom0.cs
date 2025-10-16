using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ExitFromRoom0", menuName = "Riwa/Room1/ExitFromRoom0")]
public class ExitFromRoom0 : SequencerAction
{
    private Floor1Room1LevelManager _levelManager;

    public override void Initialize(GameObject obj)
    {
        _levelManager = (Floor1Room1LevelManager)Floor1Room1LevelManager.Instance;
    }
    public override IEnumerator StartSequence(Sequencer context)
    {
        _levelManager.ExitFromRoom0();
        yield return null;
    }
}
