using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumStateSoul 
{ 
    Idle,
    Move,
    Interact,
}

public abstract class BaseStateSoul : BaseStatePawn<EnumStateSoul>
{
    /// <summary>
    /// Contient une instance du joueur, de la state machine du joueur et l'identifiant du state
    /// Chaque state a une map avec des transition, state actuel -> identifiant du state cible et va appeler l'event associé
    /// </summary>

    //FIELDS

    new protected ASoul _character;
    new protected StateMachineSoul _stateMachine;

    //PROPERTIES
    new protected ASoul Character { get => _character; set => _character = value; }

    //FUNCTIONS

    public virtual void InitState(StateMachineSoul stateMachine, EnumStateSoul enumValue, ASoul character) 
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
        _character.Animator.SetTrigger(_stateMachine.AnimationMap[_enumState]); //Lorsque je rentre dans un state, je trigger l'animation à jouer, si l'animator est bien fait, tout est clean
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
