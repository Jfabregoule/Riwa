using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[CreateAssetMenu(fileName = "Riwa Appearing", menuName = "Riwa/Dialogue/Floor1/Room4/Sequences/Riwa Appearing")]
public class SequenceActionRiwaAppearing : SequencerAction
{
    private Floor1Room4LevelManager _instance;

    public override void Initialize(GameObject obj)
    {
        _instance = (Floor1Room4LevelManager)Floor1Room4LevelManager.Instance;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _instance.Chawa.transform.SetParent(null);
        _instance.Chawa.SetActive(true);

        Vector3 initialChawaPos = _instance.Chawa.transform.position;
        Vector3 sensaPosition = GameManager.Instance.Character.transform.position;
        Vector3 targetPosition = new Vector3(sensaPosition.x - 1f, 0.5f, initialChawaPos.z);
        Vector3 finalScale = new Vector3(1f, 1f, 1f);

        Quaternion initialChawaRot = _instance.Chawa.transform.rotation;
        Quaternion chawaTargetRotation = Quaternion.Euler(0f, 90f, 0f);
        Quaternion sensaRotateTowardRiwa = Quaternion.Euler(0f, -90f, 0f);

        float elapsedTime = 0f;
        float lerpTime = 2f;

        while (elapsedTime < lerpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / lerpTime);
            GameManager.Instance.Character.transform.rotation = Quaternion.Slerp(GameManager.Instance.Character.transform.rotation, sensaRotateTowardRiwa, t);
            _instance.Chawa.transform.position = Vector3.Lerp(initialChawaPos, targetPosition, t);
            _instance.Chawa.transform.rotation = Quaternion.Slerp(initialChawaRot, chawaTargetRotation, t);
            _instance.Chawa.transform.localScale = Vector3.Lerp(_instance.Chawa.transform.localScale, finalScale, t);
            yield return null;
        }

        GameManager.Instance.Character.transform.rotation = sensaRotateTowardRiwa;
        _instance.Chawa.transform.position = targetPosition;
        _instance.Chawa.transform.rotation = chawaTargetRotation;
        _instance.Chawa.transform.localScale = finalScale;
    }
}
