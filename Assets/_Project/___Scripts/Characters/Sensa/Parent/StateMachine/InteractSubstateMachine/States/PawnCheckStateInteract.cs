using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnCheckStateInteract<TStateEnum> : PawnInteractBaseSubstate<TStateEnum>
    where TStateEnum : Enum
{
    public override void InitState(PawnInteractSubstateMachine<TStateEnum> stateMachine, EnumInteract enumValue, APawn<TStateEnum> character)
    {
        base.InitState(enumValue);
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
