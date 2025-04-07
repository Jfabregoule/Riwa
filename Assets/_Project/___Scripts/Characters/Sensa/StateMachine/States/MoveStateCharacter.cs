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

        _character.InputManager.OnInteract += OnInteract;

        _cam = GameManager.Instance.CameraHandler;
    }

    public override void ExitState()
    {
        base.ExitState();

        _character.InputManager.OnInteract -= OnInteract;
    }

    public override void UpdateState()
    {
        base.UpdateState();


        Vector2 direction = _character.InputManager.GetMoveDirection();

        Vector3 camForward = _cam.transform.forward;
        Vector3 camRight = _cam.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();
        _moveDirection = (camForward * direction.y + camRight * direction.x);
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

        Vector2 direction = _character.InputManager.GetMoveDirection();
        float magnitude = direction.magnitude;

        if (_character.InputManager.GetMoveDirection() != Vector2.zero)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Idle]);
        }

    }

    private void OnInteract()
    {
        _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Interact]);
    }
}
