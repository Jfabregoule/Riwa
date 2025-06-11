using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Orient Sensa Toward Riwa", menuName = "Riwa/Dialogue/Floor1/Room0/Sequences/Orient Sensa Toward Riwa")]
public class SequenceActionOrientSensaTowardRiwa : SequencerAction
{

    private Floor1Room0LevelManager _instance;
    public float LerpTime = 2f;
    
    public override void Initialize(GameObject obj)
    {
        _instance = (Floor1Room0LevelManager)Floor1Room0LevelManager.Instance;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        float elapsedTime = 0f;

        Quaternion initialRot = GameManager.Instance.Character.transform.rotation;
        Vector3 sensaTargetPosition = _instance.Chawa.transform.position;
        Vector3 direction = sensaTargetPosition - GameManager.Instance.Character.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        while (elapsedTime < LerpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / LerpTime);

            GameManager.Instance.Character.transform.rotation = Quaternion.Slerp(initialRot, lookRotation, t);
            yield return null;
        }

        GameManager.Instance.Character.transform.rotation = lookRotation;
    }
}
