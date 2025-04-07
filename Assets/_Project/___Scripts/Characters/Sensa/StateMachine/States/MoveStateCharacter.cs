using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveStateCharacter : BaseStateCharacter
{
    public override void InitState(StateMachineCharacter stateMachine, EnumStateCharacter enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);
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

        CameraHandler cam = GameManager.Instance.CameraHandler;

        Vector3 movement;
        movement.x = _character.Joystick.Direction.x;
        movement.y = 0;
        movement.z = _character.Joystick.Direction.y;

        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camForward * movement.z + camRight * movement.x);

        _character.LocalDirection = moveDirection;

        //_character.Rb.MovePosition(_character.Rb.position + moveDirection * _character.Speed * Time.deltaTime);

        _character.Rb.velocity = moveDirection * _character.Speed * Time.deltaTime * 100;

        if (moveDirection != Vector3.zero)
        {
            _character.Pawn.transform.forward = moveDirection;
        }

    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        Vector2 direction = new Vector2(_character.Joystick.Direction.x ,_character.Joystick.Direction.y);
        float magnitude = direction.magnitude;

        if (_character.Joystick.Direction.y == 0 && _character.Joystick.Direction.x == 0)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Idle]);
        }

    }
}
