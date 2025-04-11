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

        if (_character.InputManager.GetMoveDirection() != Vector2.zero) return;

        _stateMachine.ChangeState(_stateMachine.States[EnumHolding.IdleHolding]);
    }
}
