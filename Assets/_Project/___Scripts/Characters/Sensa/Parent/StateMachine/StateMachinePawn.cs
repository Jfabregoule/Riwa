using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachinePawn<TStateEnum, TBaseState> : BaseStateMachine<TStateEnum, TBaseState>   
    where TStateEnum : Enum
    where TBaseState : BaseStatePawn<TStateEnum>
{

    public StateMachinePawn()
    {
        _transition = new();
        States = new();
        _animationMap = new();
    }

    public override void InitStateMachine()
    {
        base.InitStateMachine();
    }

    public virtual void GoToIdle() { }

    public virtual void GoToHolding() { }

    public virtual void GoToSoul() { }

}
