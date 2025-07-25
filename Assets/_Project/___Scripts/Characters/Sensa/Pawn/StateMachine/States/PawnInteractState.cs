using System;
using System.Collections.Generic;
using UnityEngine;

public class PawnInteractState<TStateEnum> : BaseStatePawn<TStateEnum>
    where TStateEnum : Enum
{

    protected readonly List<GameObject> _colliderList = new();
    protected PawnInteractSubstateMachine<TStateEnum> _subStateMachine;

    public override void InitState(StateMachinePawn<TStateEnum, BaseStatePawn<TStateEnum>> stateMachine, TStateEnum enumValue, APawn<TStateEnum> character)
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
    }
}
