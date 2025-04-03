using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateCharacter : BaseStateCharacter
{
    private float _clock;

    public override void InitState(StateMachineCharacter stateMachine, EnumStateCharacter enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();

        _clock = 0;

        Debug.Log("ENTER");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState(float dT)
    {
        base.UpdateState(dT);

        _clock += dT;

    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        if (_character.IsChangingTime)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.ChangeTempo]);
        }

        if (_character.Joystick.Direction.y != 0 || _character.Joystick.Direction.x != 0)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Move]);
            return;
        }

        if (_clock > _character.TimeBeforeWait)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Wait]);
            return;
        }
    }
}
