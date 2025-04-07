using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumHolding
{
    IdleHolding, 
    Push,
    Pull,
    Rotate
}

public class HoldingBaseState : BaseState<EnumHolding>
{
    new protected HoldingStateMachine _stateMachine;
    protected ACharacter _character;

    public virtual void InitState(HoldingStateMachine stateMachine, EnumHolding enumValue, ACharacter character)
    {
        base.InitState(enumValue);

        _stateMachine = stateMachine;
        _character = character;

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

        CheckChangeState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();
    }
}
