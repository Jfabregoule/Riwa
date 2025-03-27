using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateCharacter : BaseStateCharacter
{

    public override void InitState(FSMCharacter stateMachine, Character character)
    {
        base.InitState(stateMachine, character);
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ChangeState()
    {
        base.ChangeState();

        if (_character.Joystick.Direction.y != 0 || _character.Joystick.Direction.x != 0)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Walk]);
        }
    }
}
