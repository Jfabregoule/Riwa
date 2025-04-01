using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStateMachine<TStateEnum> where TStateEnum : Enum
{

    /// <summary>
    /// Class parente de toutes les states machines du projet
    /// </summary>

    /*----------------------\
    |        Fields         |
    \----------------------*/

    #region Fields

    protected Dictionary<TStateEnum, BaseState<TStateEnum>> _states; //Liste des states

    protected BaseState<TStateEnum> _currentState;
    protected BaseTransitions _transition; //Class qui va contenir toutes les transitions 
    protected Dictionary<TStateEnum, string> _animationMap; //Dicionnaire pour trigger la bonne animation pour chaque state

    #endregion

    /*----------------------\
    |      Properties       |
    \----------------------*/

    #region Properties

    public BaseState<TStateEnum> CurrentState { get => _currentState; }
    public BaseTransitions Transition { get => _transition; }
    public Dictionary<TStateEnum, string> AnimationMap { get => _animationMap; }
    public Dictionary<TStateEnum, BaseState<TStateEnum>> States { get => _states; }

    #endregion

    /*----------------------\
    |        Methods        |
    \----------------------*/

    #region Methods

    public BaseStateMachine()
    {
        _transition = new BaseTransitions();
        _states = new Dictionary<TStateEnum, BaseState<TStateEnum>>();
        _animationMap = new Dictionary<TStateEnum, string>();
    }

    public virtual void InitStateMachine() { }

    public void ChangeState(BaseState<TStateEnum> newState)
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

    public void StateMachineUpdate(float dT)
    {
        _currentState.UpdateState(dT);
    }

    #endregion


}
