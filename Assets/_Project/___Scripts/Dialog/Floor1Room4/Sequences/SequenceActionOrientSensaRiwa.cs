using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Orient Sensa Toward", menuName = "Riwa/Dialogue/Floor1/Room4/Sequences/Orient Sensa Toward")]
public class SequenceActionOrientSensaRiwa : SequencerAction
{

    public bool SensaTowardRiwa = false;
    public bool SensaTowardSensaLandingPos = false;
    public bool RiwaTowardSensa = false;
    public bool RiwaTowardRiwaLandingPos = false;
    public float LerpTime = 2f;

    private Floor1Room4LevelManager _instance;

    public override void Initialize(GameObject obj)
    {
        _instance = (Floor1Room4LevelManager)Floor1Room4LevelManager.Instance;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {

        float elapsedTime = 0f;

        Vector3 sensaTargetPosition = SensaTowardRiwa ? _instance.Chawa.transform.position : SensaTowardSensaLandingPos ? _instance.SensaLandingTransform.position : Vector3.zero;
        Vector3 riwaTargetPosition = RiwaTowardSensa ? GameManager.Instance.Character.transform.position : RiwaTowardRiwaLandingPos ? _instance.RiwaLandingTransform.position : Vector3.zero;

        Quaternion sensaLookRotation = Quaternion.identity;
        Quaternion riwaLookRotation = Quaternion.identity;

        bool shouldRotateSensa = false;
        bool shouldRotateRiwa = false;

        if (sensaTargetPosition != Vector3.zero)
        {
            Vector3 sensaDirection = sensaTargetPosition - GameManager.Instance.Character.transform.position;
            sensaDirection.y = 0f;

            if (sensaDirection != Vector3.zero)
            {
                sensaLookRotation = Quaternion.LookRotation(sensaDirection);
                shouldRotateSensa = true;
            }
        }

        if (riwaTargetPosition != Vector3.zero)
        {
            Vector3 riwaDirection = riwaTargetPosition - _instance.Chawa.transform.position;
            riwaDirection.y = 0f;

            if (riwaDirection != Vector3.zero)
            {
                riwaLookRotation = Quaternion.LookRotation(riwaDirection);
                shouldRotateRiwa = true;
            }
        }

        while (elapsedTime < LerpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / LerpTime);

            if (shouldRotateSensa)
                GameManager.Instance.Character.transform.rotation = Quaternion.Slerp(GameManager.Instance.Character.transform.rotation, sensaLookRotation, t);

            if (shouldRotateRiwa)
                _instance.Chawa.transform.rotation = Quaternion.Slerp(_instance.Chawa.transform.rotation, riwaLookRotation, t);

            yield return null;
        }

        if (shouldRotateSensa)
            GameManager.Instance.Character.transform.rotation = sensaLookRotation;

        if (shouldRotateRiwa)
            _instance.Chawa.transform.rotation = riwaLookRotation;
    }
}
