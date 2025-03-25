using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WalkStateCharacter : BaseStateCharacter
{
    new public void InitState(FSMCharacter stateMachine, Character character)
    {
        base.InitState(stateMachine, character);

        //Pas de transition
    }

    new public void EnterState()
    {
        base.EnterState();
    }

    new public void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        Vector3 movement;
        movement.x = _character.Joystick.Direction.x;
        movement.y = 0;
        movement.z = _character.Joystick.Direction.y;

        Vector3 direction = Vector3.Normalize(new Vector3(movement.x, 0, movement.z));

        _character.Rb.velocity = movement * _character.Speed;

        ChangeState();
    }

    public override void ChangeState()
    {
        Vector2 direction = new Vector2(_character.Joystick.Direction.x ,_character.Joystick.Direction.y);
        float magnitude = direction.magnitude;

        if (magnitude > 0.4f)
        {
            _stateMachine.ChangeState(_stateMachine._states[EnumStateCharacter.Run]);
        }

        if (_character.Joystick.Direction.y == 0 && _character.Joystick.Direction.x == 0)
        {
            _stateMachine.ChangeState(_stateMachine._states[EnumStateCharacter.Idle]);
        }

    }
}
