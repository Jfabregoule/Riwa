using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hide Riwa", menuName = "Riwa/Dialogue/Floor1/Room4/Sequences/Hide Riwa")]
public class SequenceActionHideRiwa : SequencerAction
{

    private Floor1Room4LevelManager _instance;
    private DialogueSystem _dialogueSystem;

    public override void Initialize(GameObject obj)
    {
        _instance = (Floor1Room4LevelManager)Floor1Room4LevelManager.Instance;
        _dialogueSystem = DialogueSystem.Instance;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _instance.Chawa.transform.SetParent(GameManager.Instance.Character.transform);
        Vector3 initialPos = _instance.Chawa.transform.position;
        Vector3 targetPos = GameManager.Instance.Character.transform.position;
        Vector3 finalScale = new Vector3(0f, 0f, 0f);

        float elapsedTime = 0f;

        while (elapsedTime < 2f)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / 2f);
            _instance.Chawa.transform.position = Vector3.Lerp(initialPos, targetPos, t);
            _instance.Chawa.transform.localScale = Vector3.Lerp(_instance.Chawa.transform.localScale, finalScale, t);
            yield return null;
        }

        _instance.Chawa.transform.position = targetPos;
        _instance.Chawa.transform.localScale = finalScale;
        _instance.Chawa.SetActive(false);
        //_instance.IsTutorialDone = true;
        //SaveSystem.Instance.SaveElement<bool>("Room4TutorialDone", true);
        _dialogueSystem.EventRegistery.Invoke(WaitDialogueEventType.RiwaHiddingIntoSensa);
    }
}
