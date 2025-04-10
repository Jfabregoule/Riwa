using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumInteract
{
    Check,
    Move,
    Action,
    StandBy
}

public class PawnInteractBaseSubstate<TStateEnum> : BaseState<EnumInteract>
    where TStateEnum : Enum //Enum pour soul et character
{
    protected APawn<TStateEnum> _character;
    new protected PawnInteractSubstateMachine<TStateEnum> _stateMachine;

    public virtual void InitState(PawnInteractSubstateMachine<TStateEnum> stateMachine, EnumInteract enumValue, APawn<TStateEnum> character)
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
