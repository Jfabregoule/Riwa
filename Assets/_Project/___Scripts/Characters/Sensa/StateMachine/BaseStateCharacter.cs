using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    SoulIdle,
    SoulWalk,
    Fall,
    Rotate,
    Respawn
}

public abstract class BaseStateCharacter : BaseState<EnumStateCharacter>
{
    /// <summary>
    /// Contient une instance du joueur, de la state machine du joueur et l'identifiant du state
    /// Chaque state a une map avec des transition, state actuel -> identifiant du state cible et va appeler l'event associé
    /// </summary>

    //FIELDS

    protected ACharacter _character;
    new protected StateMachineCharacter _stateMachine;

    //PROPERTIES
    protected ACharacter Character { get => _character; set => _character = value; }

    //FUNCTIONS

    public virtual void InitState(StateMachineCharacter stateMachine, EnumStateCharacter enumValue, ACharacter character) 
    {
        base.InitState(enumValue);

        //Je set la state machine dans le baseStateCHaracter et pas plus haut dans l'héritage car les templates ont leurs limites
        _stateMachine = stateMachine;
        _character = character;
        _transitionMap = new Dictionary<EnumStateCharacter, Transition>();
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

    public override void UpdateState(float dT)
    {
        //code commun à tous les states
        base.UpdateState(dT);
        ChangeState();
    }

    public override void ChangeState()
    {
        //code commun à tous les states
        //Ici on mettra les conditions et tout ce qui concerne les changements de state
        base.ChangeState();
    }

}
