using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sensa toward Riwa", menuName = "Riwa/Dialogue/Floor1/Room0/Sequences/Sensa toward Riwa")]
public class SequenceActionRoom0SensaTowardRiwa : SequencerAction
{

    private Floor1Room0LevelManager _instance;

    public override void Initialize(GameObject obj)
    {
        _instance = (Floor1Room0LevelManager)Floor1Room0LevelManager.Instance;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        Vector3 initialPos = GameManager.Instance.Character.transform.position;
        Vector3 landPos = _instance.SensaLandPos.position;

        float elapsedTime = 0f;
        float lerpTime = 3f;

        while(elapsedTime < lerpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / lerpTime);
            GameManager.Instance.Character.transform.position = Vector3.Lerp(initialPos, landPos, t);
            yield return null;
        }

        GameManager.Instance.Character.transform.position = landPos;
    }
}
