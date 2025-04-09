using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumInteract
{
    StandBy,
    Check,
    Action,
    Move
}

public class InteractBaseState<TStateEnum> : BaseState<EnumInteract>
    where TStateEnum : Enum
{
    new protected InteractStateMachine<TStateEnum> _stateMachine;
    protected APawn<TStateEnum> _character;

    public virtual void InitState(InteractStateMachine<TStateEnum> stateMachine, EnumInteract enumValue, APawn<TStateEnum> character)
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
