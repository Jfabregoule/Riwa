using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Show Room4", menuName = "Riwa/Dialogue/Floor1/Room4/Sequences/Show Room 4")]
public class SequenceActionShowRoom4 : SequencerAction
{

    private Floor1Room4LevelManager _instance;

    public override void Initialize(GameObject obj)
    {
        _instance = (Floor1Room4LevelManager)Floor1Room4LevelManager.Instance;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        yield return new WaitForSeconds(1.5f);
        int currentIndex = 0;

        _instance.FresqueCameras[currentIndex].Priority = 20;

        for (int nextIndex = 1; nextIndex < _instance.FresqueCameras.Count; nextIndex++)
        {
            yield return new WaitForSeconds(2.5f);

            _instance.FresqueCameras[currentIndex].Priority = 0;

            _instance.FresqueCameras[nextIndex].Priority = 20;

            currentIndex = nextIndex;
        }
        yield return new WaitForSeconds(1.8f);
    }
}
