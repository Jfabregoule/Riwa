using System;
using UnityEngine;

public class ParentIdleState<TStateEnum> : BaseStatePawn<TStateEnum>
    where TStateEnum : Enum
{
    protected float _clock;

    public override void InitState(StateMachinePawn<TStateEnum, BaseStatePawn<TStateEnum>> stateMachine, TStateEnum enumValue, APawn<TStateEnum> character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();
        _clock = 0;
        //_character.Rb.velocity = Vector3.zero;
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
    }
}
