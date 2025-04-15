using System;
using System.Collections.Generic;

public abstract class BaseStateMachine<TStateEnum, TBaseState> 
    where TStateEnum : Enum
    where TBaseState : BaseState<TStateEnum>
{

    /// <summary>
    /// Class parente de toutes les states machines du projet
    /// Chaque state machine aura un enum associé lui permettant de manipuler les states via le dictionnaire
    /// Il faudra dont renseigner :
    ///     TStateEnum = type de l'enum utilisé
    ///     TBaseState = type de la base class des states
    /// </summary>

    /*----------------------\
    |        Fields         |
    \----------------------*/

    #region Fields

    public Dictionary<TStateEnum, TBaseState> States; //Liste des states

    protected TBaseState _currentState;
    protected BaseTransitions _transition; //Class qui va contenir toutes les transitions 
    protected Dictionary<TStateEnum, string> _animationMap; //Dicionnaire pour trigger la bonne animation pour chaque state

    #endregion

    /*----------------------\
    |      Properties       |
    \----------------------*/

    #region Properties

    public TBaseState CurrentState { get => _currentState; }
    public BaseTransitions Transition { get => _transition; }
    public Dictionary<TStateEnum, string> AnimationMap { get => _animationMap; }
    
    #endregion

    /*----------------------\
    |        Methods        |
    \----------------------*/

    #region Methods

    public BaseStateMachine()
    {
        _transition = new BaseTransitions();
        //_states = new Dictionary<TStateEnum, BaseState<TStateEnum>>();
        _animationMap = new Dictionary<TStateEnum, string>();
    }

    public virtual void InitStateMachine() { }

    public virtual void InitState(TBaseState initState)
    {
        _currentState = initState;
        _currentState.EnterState();
    }

    public virtual void ChangeState(TBaseState newState)
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

    public virtual void StateMachineUpdate()
    {
        _currentState.UpdateState();
    }

    public virtual void StateMachineFixedUpdate()
    {
        _currentState.FixedUpdateState();
    }

    #endregion


}
