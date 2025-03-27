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
    SoulRun,
    SoulInteract
}

public abstract class BaseStateCharacter
{
    //FIELDS

    protected FSMCharacter                                  _stateMachine;
    protected EnumStateCharacter                            _enumState; //L'identité du state soula forme d'un enum 
    protected Character                                     _character;

    //Liste des transitions entre les states
    public delegate void Transition();
    protected Dictionary<EnumStateCharacter, Transition>    _transitionMap;

    //PROPERTIES

    public Dictionary<EnumStateCharacter, Transition> TransitionMap { get => _transitionMap; }
    public EnumStateCharacter EnumState { get => _enumState;}
    public FSMCharacter Character { get => _stateMachine;}

    //FUNCTIONS

    public virtual void InitState(FSMCharacter stateMachine, Character character) 
    {
        _stateMachine = stateMachine;
        _enumState = EnumStateCharacter.Idle;
        _character = character;
        _transitionMap = new Dictionary<EnumStateCharacter, Transition>();
    }

    public virtual void EnterState() 
    {
        //_character.Animator.SetTrigger(_stateMachine.AnimationMap[_enumState]); //Lorsque je rentre dans un state, je trigger l'animation à jouer, si l'animator est bien fait, tout est clean
    }

    public virtual void ExitState()
    {
        //code commun à tous les states
    }

    public virtual void UpdateState()
    {
        //code commun à tous les states
        ChangeState();
    }

    public virtual void ChangeState()
    {
        //code commun à tous les states
        //Ici on mettra les conditions et tout ce qui concerne les changements de state
    }

}
