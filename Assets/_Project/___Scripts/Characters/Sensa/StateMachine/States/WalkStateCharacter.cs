using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WalkStateCharacter : BaseStateCharacter
{
    public override void InitState(FSMCharacter stateMachine, Character character)
    {
        _stateMachine = stateMachine;
        _enumState = EnumStateCharacter.Walk;

        //Pas de transition
    }

    public override void EnterState()
    {

    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        Vector3 movement;
        movement.x = _character.Joystick.Direction.x;
        movement.z = _character.Joystick.Direction.y;

        Vector3 direction = Vector3.Normalize(new Vector3(movement.x, 0, movement.z));


        _character.Rb.velocity = direction * _character.Speed;

        //_character.transform.position.

        //ChangeState();
    }

    public override void ChangeState()
    {
        Vector2 direction = new Vector2(_character.Joystick.Direction.x ,_character.Joystick.Direction.y);
        direction.Normalize();
        float magnitude = direction.magnitude;

        if (magnitude > 0.5f)
        {
            _stateMachine.ChangeState(_stateMachine._states[EnumStateCharacter.Run]);
        }

        if (_character.Joystick.Direction.y == 0 && _character.Joystick.Direction.x == 0)
        {
            _stateMachine.ChangeState(_stateMachine._states[EnumStateCharacter.Idle]);
        }

    }
}
