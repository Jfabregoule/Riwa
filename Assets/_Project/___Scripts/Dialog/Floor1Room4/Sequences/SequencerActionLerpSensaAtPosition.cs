using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lerp Sensa At Position", menuName = "Riwa/Dialogue/Floor1/Room4/Sequences/Lerp Sensa At Position")]
public class SequencerActionLerpSensaAtPosition : SequencerAction
{
    public float MoveDuration = 3f;
    public bool MoveWithRiwa = false;
    public Action SensaLandAtFragment;
    private Floor1Room4LevelManager _instance;
    private DialogueSystem _dialogueSystem;

    public override void Initialize(GameObject obj)
    {
        _instance = (Floor1Room4LevelManager)Floor1Room4LevelManager.Instance;
        _dialogueSystem = DialogueSystem.Instance;
        _dialogueSystem.EventRegistery.Register(WaitDialogueEventType.WaitForSensaLandingAtFragment, SensaLandAtFragment);
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        Vector3 initialSensaPosition = GameManager.Instance.Character.transform.position;
        Vector3 targetSensaPosition = _instance.SensaLandingTransform.position;

        Vector3 initialRiwaPosition = _instance.Chawa.transform.position;
        Vector3 targetRiwaPosition = _instance.RiwaLandingTransform.position;

        float elapsedTime = 0f;

        while(elapsedTime < MoveDuration)
        {
            GameManager.Instance.Character.Animator.SetBool("Move", true);
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / MoveDuration);
            GameManager.Instance.Character.transform.position = Vector3.Lerp(initialSensaPosition, targetSensaPosition, t);
            if (MoveWithRiwa) _instance.Chawa.transform.position = Vector3.Lerp(initialRiwaPosition, targetRiwaPosition, t);
            yield return null;
        }

        GameManager.Instance.Character.Animator.SetBool("Move", false);
        GameManager.Instance.Character.transform.position = targetSensaPosition;
        if (MoveWithRiwa) _instance.Chawa.transform.position = targetRiwaPosition;
        _dialogueSystem.EventRegistery.Invoke(WaitDialogueEventType.WaitForSensaLandingAtFragment);
    }
}
