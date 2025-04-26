using System;
using System.Diagnostics;
public class BaseStatePawn<TStateEnum> : BaseState<TStateEnum>
    where TStateEnum : Enum
{
    //FIELDS

    protected APawn<TStateEnum> _character;
    new protected StateMachinePawn<TStateEnum, BaseStatePawn<TStateEnum>> _stateMachine;

    //PROPERTIES
    protected APawn<TStateEnum> Character { get => _character; set => _character = value; }

    //FUNCTIONS

    public virtual void InitState(StateMachinePawn<TStateEnum, BaseStatePawn<TStateEnum>> stateMachine, TStateEnum enumValue, APawn<TStateEnum> character)
    {
        base.InitState(enumValue);

        //Je set la state machine dans le baseStateCHaracter et pas plus haut dans l'héritage car les templates ont leurs limites
        _stateMachine = stateMachine;
        _character = character;
        _transitionMap = new();
    }

    public override void EnterState()
    {
        base.EnterState();
        if (_character.Animator != null && Helpers.HasParameter(_stateMachine.AnimationMap[_enumState], _character.Animator)) { 
            _character.Animator.SetBool(_stateMachine.AnimationMap[_enumState], true); //Lorsque je rentre dans un state, je trigger l'animation à jouer, si l'animator est bien fait, tout est clean  
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        //code commun à tous les states
        if (_character.Animator != null && Helpers.HasParameter(_stateMachine.AnimationMap[_enumState], _character.Animator))
        {
            _character.Animator.SetBool(_stateMachine.AnimationMap[_enumState], false); //Lorsque je rentre dans un state, je trigger l'animation   jouer, si l'animator est bien fait, tout est clean  
        }
    }

    public override void UpdateState()
    {
        //code commun à tous les states
        base.UpdateState();
    }

    public override void CheckChangeState()
    {
        //code commun à tous les states
        //Ici on mettra les conditions et tout ce qui concerne les changements de state
        base.CheckChangeState();
    }
}
