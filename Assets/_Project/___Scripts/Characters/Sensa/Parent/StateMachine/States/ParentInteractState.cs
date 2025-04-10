using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentInteractState<TStateEnum> : BaseStatePawn<TStateEnum>
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
        _subStateMachine.ChangeState(_subStateMachine.States[EnumInteract.Check]);
    }

    public override void ExitState()
    {
        base.ExitState();
        _subStateMachine.ChangeState(_subStateMachine.States[EnumInteract.StandBy]);
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
