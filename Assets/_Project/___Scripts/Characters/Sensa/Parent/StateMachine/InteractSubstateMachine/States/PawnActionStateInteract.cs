using System;
using UnityEngine;

public class PawnActionStateInteract<TStateEnum> : PawnInteractBaseSubstate<TStateEnum>
    where TStateEnum : Enum
{
    protected float _animClock;
    protected float _animationTime = 0;

    public override void InitState(PawnInteractSubstateMachine<TStateEnum> stateMachine, EnumInteract enumValue, APawn<TStateEnum> character)
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
        _subStateMachine.CurrentObjectInteract.GetComponent<IInteractable>().Interact();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _animClock += Time.deltaTime;
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();
    }
}
