using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveStateCharacter : BaseStateCharacter
{

    private CameraHandler _cam;
    private Vector3 _moveDirection;

    public override void InitState(StateMachineCharacter stateMachine, EnumStateCharacter enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();

        _cam = GameManager.Instance.CameraHandler;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Vector3 movement;

        movement.x = _character.Joystick.Direction.x;
        movement.y = 0;
        movement.z = _character.Joystick.Direction.y;

        Vector3 camForward = _cam.transform.forward;
        Vector3 camRight = _cam.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        _moveDirection = (camForward * movement.z + camRight * movement.x).normalized;
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        _character.Rb.velocity = _moveDirection * _character.Speed * Time.deltaTime * 100;

        if (_moveDirection != Vector3.zero)
        {
            _character.Pawn.transform.forward = _moveDirection;
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
