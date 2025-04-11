using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStateHolding : HoldingBaseState
{
    private int _sens;
    private IMovable _movable;
    public int Sens { get => _sens; set => _sens = value; }

    public override void InitState(HoldingStateMachine stateMachine, EnumHolding enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();
        if (_character.HoldingObject.TryGetComponent(out IMovable movable))
        {
            _movable = movable;
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _movable.Move(Sens * _character.transform.forward);
        _character.transform.position += Sens * _character.transform.forward * _movable.MoveSpeed * Time.deltaTime * 10;
        
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        Vector2 joystickDir = _character.InputManager.GetMoveDirection();

        if (joystickDir == Vector2.zero) {
            _stateMachine.ChangeState(_stateMachine.States[EnumHolding.IdleHolding]);
            return;
        }

        Vector3 inputDir = new Vector3(joystickDir.x, 0, joystickDir.y).normalized;

        Vector3 playerForward = _character.transform.forward;
        Vector3 forward = new Vector3(Mathf.Round(playerForward.x), 0, Mathf.Round(playerForward.z)).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, forward);

        float dotForward = Vector3.Dot(forward, inputDir);
        float dotRight = Vector3.Dot(right, inputDir);

        if (Mathf.Abs(dotForward) > Mathf.Abs(dotRight))
        {
            if (dotForward > 0.5f)
            {
                Sens = 1;
            }
            else
            {
                Sens = -1;
            }
        }
        else
        {
            if (!_character.HoldingObject.TryGetComponent(out IRotatable rotatable)) return;
            if (dotRight > 0.5f)
            {
                //Rotate Droite
                ((RotateStateHolding)_stateMachine.States[EnumHolding.Rotate]).Sens = 1;
                _stateMachine.ChangeState(_stateMachine.States[EnumHolding.Rotate]);
            }
            else
            {
                //Rotate Gauche
                ((RotateStateHolding)_stateMachine.States[EnumHolding.Rotate]).Sens = -1;
                _stateMachine.ChangeState(_stateMachine.States[EnumHolding.Rotate]);
            }
        }   
    }
}
