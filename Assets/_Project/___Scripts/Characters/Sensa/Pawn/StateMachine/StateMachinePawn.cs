using System;
using TMPro;

public abstract class StateMachinePawn<TStateEnum, TBaseState> : BaseStateMachine<TStateEnum, TBaseState>   
    where TStateEnum : Enum
    where TBaseState : BaseStatePawn<TStateEnum>
{

    protected APawn<TStateEnum> _character;

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

    public void InitStateMachine(APawn<TStateEnum> character)
    {
        base.InitStateMachine();
        _character = character;
    }

    public override void InitState(TBaseState initState)
    {
        base.InitState(initState);
        if (_character.Animator != null)
        {
            _character.Animator.ResetTrigger(_animationMap[initState.EnumState]);
        }
    }

    public virtual void GoToIdle() { }

    public virtual void GoToHolding() { }

    public virtual void GoToSoul() { }

}
