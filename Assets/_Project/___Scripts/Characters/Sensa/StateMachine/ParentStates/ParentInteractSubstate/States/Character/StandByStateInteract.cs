using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandByStateInteract<TStateEnum> : InteractBaseState<TStateEnum>
    where TStateEnum : Enum
{
    public override void InitState(InteractStateMachine<TStateEnum> stateMachine, EnumInteract enumValue, APawn<TStateEnum> character)
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
