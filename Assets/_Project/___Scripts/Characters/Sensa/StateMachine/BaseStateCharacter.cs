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

public abstract class BaseStateCharacter : MonoBehaviour
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

    public abstract void InitState(FSMCharacter stateMachine, Character character);
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract void ChangeState();

}
