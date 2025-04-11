using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleHoldingStateHolding : HoldingBaseState
{
    public override void InitState(HoldingStateMachine stateMachine, EnumHolding enumValue, ACharacter character)
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

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        Vector2 joystickDir = _character.InputManager.GetMoveDirection();

        if (joystickDir == Vector2.zero) return;

        Vector3 inputDir = new Vector3(joystickDir.x, 0, joystickDir.y).normalized;

        Vector3 playerForward = _character.transform.forward;
        Vector3 forward = new Vector3(Mathf.Round(playerForward.x), 0, Mathf.Round(playerForward.z)).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, forward);

        float dotForward = Vector3.Dot(forward, inputDir);
        float dotRight = Vector3.Dot(right, inputDir);

        if (Mathf.Abs(dotForward) > Mathf.Abs(dotRight))
        {
            if (!_character.HoldingObject.TryGetComponent(out IMovable movable)) return;
            if (dotForward > 0.5f)
            {
                //Pull
                ((MoveStateHolding)_stateMachine.States[EnumHolding.Move]).Sens = 1;
                _stateMachine.ChangeState(_stateMachine.States[EnumHolding.Move]);
            } 
            else 
            {
                //Push
                ((MoveStateHolding)_stateMachine.States[EnumHolding.Move]).Sens = -1;
                _stateMachine.ChangeState(_stateMachine.States[EnumHolding.Move]);
            }
        }
        else
        {
            if (!_character.HoldingObject.TryGetComponent(out IRotatable rotatable)) return;
            if (dotForward > 0.5f)
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
