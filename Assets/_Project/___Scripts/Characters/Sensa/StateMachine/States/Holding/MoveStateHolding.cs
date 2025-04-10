using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStateHolding : HoldingBaseState
{
    private int _sens;
    public int Sens { get => _sens; set => value = _sens; }
    public override void InitState(HoldingStateMachine stateMachine, EnumHolding enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();
        //if (_character.HoldingObject.TryGetComponent(out IMovable movable))
        //{
        //    movable.Move(Sens);
        //}
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

        _stateMachine.ChangeState(_stateMachine.States[EnumHolding.IdleHolding]);
    }
}
