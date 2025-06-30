using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Update to End", menuName = "Riwa/Dialogue/Floor1/Room1/Sequences/End Advancement")]
public class SequenceActionUpdateAvancement : SequencerAction
{

    private Floor1Room1LevelManager _levelManager;

    public override void Initialize(GameObject obj)
    {
        _levelManager = (Floor1Room1LevelManager)Floor1Room1LevelManager.Instance;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _levelManager.UpdateAdvancement(EnumAdvancementRoom1.End);
        yield return null;
    }
}
