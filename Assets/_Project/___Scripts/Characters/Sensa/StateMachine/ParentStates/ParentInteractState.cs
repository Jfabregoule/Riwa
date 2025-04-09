using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentInteractState<TStateEnum> : BaseStatePawn<TStateEnum>
    where TStateEnum : Enum
{

    protected readonly List<GameObject> _colliderList = new();
    protected InteractStateMachine<TStateEnum> _subStateMachine;

    public override void InitState(StateMachinePawn<TStateEnum, BaseStatePawn<TStateEnum>> stateMachine, TStateEnum enumValue, APawn<TStateEnum> character)
    {
        base.InitState(stateMachine, enumValue, character);
        _subStateMachine = new();
        _subStateMachine.InitStateMachine(character);
        _subStateMachine.InitState(_subStateMachine.States[EnumInteract.StandBy]);
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
