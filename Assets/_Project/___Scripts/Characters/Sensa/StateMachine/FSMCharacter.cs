using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMCharacter
{
    public  Dictionary<EnumStateCharacter, BaseStateCharacter>  _states; //Liste des states

    private BaseStateCharacter                                  _currentState;
    private Transitions                                         _transition; //Class qui va contenir toutes les transitions 
    private Dictionary<EnumStateCharacter, string>              _animationMap; //Dicionnaire pour trigger la bonne animation pour chaque state

    public BaseStateCharacter CurrentState { get => _currentState; }
    public Transitions Transition { get => _transition;}
    public Dictionary<EnumStateCharacter, string> AnimationMap { get => _animationMap; }

    public FSMCharacter() { 
        _transition = new Transitions();
        _states = new Dictionary<EnumStateCharacter, BaseStateCharacter>();
    }

    public void InitStateMachine(Character character)
    {
        _states[EnumStateCharacter.Idle] = new IdleStateCharacter();
        _states[EnumStateCharacter.Idle].InitState(this, character);

        _states[EnumStateCharacter.Walk] = new WalkStateCharacter();
        _states[EnumStateCharacter.Walk].InitState(this, character);

        _states[EnumStateCharacter.Run] = new RunStateCharacter();
        _states[EnumStateCharacter.Run].InitState(this, character);

        _states[EnumStateCharacter.Interact] = new InteractStateCharacter();
        _states[EnumStateCharacter.Interact].InitState(this, character);
    }

    public void InitState(BaseStateCharacter initState)
    {
        _currentState = initState;
        _currentState.EnterState();
    }

    public void ChangeState(BaseStateCharacter newState)
    {
        //A refactor ça prend ptet trop de ressources
        if (_currentState.TransitionMap.ContainsKey(newState.EnumState))
        {
            _currentState.TransitionMap[newState.EnumState]?.Invoke();
        }
        _currentState.ExitState();
        _currentState = newState;
        _currentState.EnterState();


    }

    public void StateMachineUpdate()
    {
        _currentState.UpdateState();
    }

}
