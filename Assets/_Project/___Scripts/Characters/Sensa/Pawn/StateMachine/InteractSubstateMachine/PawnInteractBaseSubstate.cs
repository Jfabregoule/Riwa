using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    protected PawnInteractSubstateMachine<TStateEnum> _subStateMachine;

    public virtual void InitState(PawnInteractSubstateMachine<TStateEnum> stateMachine, EnumInteract enumValue, APawn<TStateEnum> character)
    {
        base.InitState(enumValue);

        _subStateMachine = stateMachine;
        _character = character;

    }

    public override void EnterState()
    {
        base.EnterState();
        if (_character.Animator != null)
        {
            _character.Animator.SetTrigger(_subStateMachine.AnimationMap[_enumState]); //Lorsque je rentre dans un state, je trigger l'animation à jouer, si l'animator est bien fait, tout est clean  
        }
    }

    public override void ExitState()
    {
        base.ExitState();

        if (_character.Animator != null)
        {
            _character.Animator.ResetTrigger(_subStateMachine.AnimationMap[_enumState]); //Lorsque je rentre dans un state, je trigger l'animation   jouer, si l'animator est bien fait, tout est clean  
        }
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
