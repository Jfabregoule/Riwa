using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeTime", menuName = "Riwa/GenericAction/ChangeTime")]
public class SequenceActionChangeTime : SequencerAction
{
    private ACharacter _character;

    public override void Initialize(GameObject obj)
    {
        _character = GameManager.Instance.Character;
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        _character.StateMachine.ChangeState(_character.StateMachine.States[EnumStateCharacter.ChangeTempo]);
        yield return null;
    }

}