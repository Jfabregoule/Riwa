using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumStateSoul 
{ 
    Idle,
    Move,
}

public abstract class BaseStateSoul : BaseState<EnumStateSoul>
{
    /// <summary>
    /// Contient une instance du joueur, de la state machine du joueur et l'identifiant du state
    /// Chaque state a une map avec des transition, state actuel -> identifiant du state cible et va appeler l'event associé
    /// </summary>

    //FIELDS

    protected ASoul _soul;
    new protected StateMachineSoul _stateMachine;

    //PROPERTIES
    protected ASoul Soul { get => _soul; set => _soul = value; }

    //FUNCTIONS

    public virtual void InitState(StateMachineSoul stateMachine, EnumStateSoul enumValue, ASoul character) 
    {
        base.InitState(enumValue);

        //Je set la state machine dans le baseStateCHaracter et pas plus haut dans l'héritage car les templates ont leurs limites
        _stateMachine = stateMachine;
        _soul = character;
        _transitionMap = new Dictionary<EnumStateSoul, Transition>();
    }

    public override void EnterState() 
    {
        base.EnterState();
        _soul.Animator.SetTrigger(_stateMachine.AnimationMap[_enumState]); //Lorsque je rentre dans un state, je trigger l'animation à jouer, si l'animator est bien fait, tout est clean
    }

    public override void ExitState()
    {
        base.ExitState();
        //code commun à tous les states
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
