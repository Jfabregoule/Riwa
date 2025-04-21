using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Change Fresque Camera Priority", menuName = "Riwa/Dialogue/Floor1/Room4/Sequences/Fresque Camera")]
public class SequencerActionChangeFresqueCam : SequencerAction
{
    public bool StopShowingFresque;
    public int FresqueCameraID;
    private Floor1Room4LevelManager _instance;

    public override void Initialize(GameObject obj)
    {
        _instance = (Floor1Room4LevelManager)Floor1Room4LevelManager.Instance;
    }
    public override IEnumerator StartSequence(Sequencer context)
    {
        for(int i = 0; i < _instance.FresqueCameras.Count; i++)
        {
            if (i == FresqueCameraID)
                _instance.FresqueCameras[i].Priority = StopShowingFresque ? 0 : 20;
            else
                _instance.FresqueCameras[i].Priority = 0;
        }
        yield return null;
    }
}
