using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumStateCharacter
{
    Idle,
    Walk,
    Run,
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

    private Character _character;

    //PROPERTIES
    protected Character Character1 { get => _character; set => _character = value; }

    //FUNCTIONS

    public virtual void InitState(StateMachineCharacter stateMachine, EnumStateCharacter enumValue, Character character) 
    {
        base.InitState(stateMachine, enumValue);

        _character = character;
        _transitionMap = new Dictionary<EnumStateCharacter, Transition>();
    }

    new public virtual void EnterState() 
    {
        base.EnterState();
        _character.Animator.SetTrigger(_stateMachine.AnimationMap[_enumState]); //Lorsque je rentre dans un state, je trigger l'animation à jouer, si l'animator est bien fait, tout est clean
    }

    new public virtual void ExitState()
    {
        base.ExitState();
        //code commun à tous les states
    }

    new public virtual void UpdateState(float dT)
    {
        //code commun à tous les states
        base.UpdateState(dT);
        ChangeState();
    }

    new public virtual void ChangeState()
    {
        //code commun à tous les states
        //Ici on mettra les conditions et tout ce qui concerne les changements de state
        base.ChangeState();
    }

}
