using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunStateCharacter : BaseStateCharacter
{
    public override void InitState(FSMCharacter stateMachine, Character character)
    {
        base.InitState(stateMachine, character);

        _enumState = EnumStateCharacter.Run;

    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState(float dT)
    {
        base.UpdateState(dT);

        Vector3 movement;
        movement.x = _character.Joystick.Direction.x;
        movement.y = 0;
        movement.z = _character.Joystick.Direction.y;

        Vector3 direction = Vector3.Normalize(new Vector3(movement.x, 0, movement.z));

        _character.Rb.velocity = movement * _character.Speed;
        _character.Pawn.transform.forward = direction;

    }

    public override void ChangeState()
    {
        base.ChangeState();

        Vector2 direction = new Vector2(_character.Joystick.Direction.x, _character.Joystick.Direction.y);
        float magnitude = direction.magnitude;

        if (magnitude < 0.4f)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Walk]);
        }
    }
}
