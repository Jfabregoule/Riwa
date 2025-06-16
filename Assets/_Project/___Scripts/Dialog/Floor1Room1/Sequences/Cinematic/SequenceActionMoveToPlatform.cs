using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveTo Platform", menuName = "Riwa/Dialogue/Floor1/Room1/Sequences/MoveTo Platform")]
public class SequenceActionMoveToPlatform : SequencerAction
{

    private ACharacter _character;
    private Floor1Room1LevelManager _instance;

    public override void Initialize(GameObject obj)
    {
        _character = GameManager.Instance.Character;
        _instance = (Floor1Room1LevelManager)Floor1Room1LevelManager.Instance;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        float elapsedTime = 0f;
        float lerpTime = 2f;

        Vector3 initialPos = _character.transform.position;
        Vector3 targetPos = _instance.EndGameCinematic.ElevatorPlatformTransform.position;

        targetPos.y = _character.transform.position.y;

        _character.Animator.SetBool("MoveTo", true);
        while (elapsedTime < lerpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / lerpTime);
            _character.transform.position = Vector3.Lerp(initialPos, targetPos, t);
            yield return null;
        }

        _character.transform.position = targetPos;
        _character.Animator.SetBool("MoveTo", false);
    }
}
