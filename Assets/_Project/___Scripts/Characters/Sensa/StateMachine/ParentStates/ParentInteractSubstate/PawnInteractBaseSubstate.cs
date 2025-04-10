using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnInteractBaseSubstate<TStateEnum> : BaseState<EnumInteract>
    where TStateEnum : Enum //Enum pour soul et character
{
    public virtual void InitState(PawnInteractSubstateMachine<EnumInteract> stateMachine, EnumInteract enumValue, APawn<TStateEnum> character)
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
