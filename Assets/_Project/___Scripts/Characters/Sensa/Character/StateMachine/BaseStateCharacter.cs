using System;
using System.Collections.Generic;

public enum EnumStateCharacter
{
    Idle,
    Move,
    Cinematic,
    Wait,
    ChangeTempo,
    Interact,
    Holding,
    Push,
    Pull,
    Soul,
    Fall,
    Rotate,
    Respawn
}

public abstract class BaseStateCharacter<TStateEnum> : BaseStatePawn<TStateEnum>
    where TStateEnum : Enum
{
    /// <summary>
    /// Contient une instance du joueur, de la state machine du joueur et l'identifiant du state
    /// Chaque state a une map avec des transition, state actuel -> identifiant du state cible et va appeler l'event associé
    /// </summary>

    //FIELDS


    //PROPERTIES



    //FUNCTIONS

    public override void InitState(StateMachinePawn<TStateEnum, BaseStatePawn<TStateEnum>> stateMachine, TStateEnum enumValue, APawn<TStateEnum> character) 
    {
        base.InitState(stateMachine, enumValue, character); 

        //Je set la state machine dans le baseStateCHaracter et pas plus haut dans l'héritage car les templates ont leurs limites
        _stateMachine = stateMachine;
        _character = character;
        _transitionMap = new Dictionary<TStateEnum, Transition>();
    }

    public override void EnterState() 
    {
        base.EnterState();
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
