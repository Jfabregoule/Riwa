using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BaseState<TStateEnum> where TStateEnum : Enum
{
    ///<summary>
    /// State parent de touts les states de toutes les states machines du monde
    /// </summary>

    /*----------------------\
    |        Fields         |
    \----------------------*/

    #region Fields

    protected BaseStateMachine<TStateEnum> _stateMachine;
    protected TStateEnum _enumState; //L'identité du state soula forme d'un enum 

    //Liste des transitions entre les states
    public delegate void Transition();
    protected Dictionary<TStateEnum, Transition> _transitionMap;

    #endregion

    /*----------------------\
    |      Properties       |
    \----------------------*/

    #region Properties

    public Dictionary<TStateEnum, Transition> TransitionMap { get => _transitionMap; }
    public TStateEnum EnumState { get => _enumState; }
    public BaseStateMachine<TStateEnum> Character { get => _stateMachine; }

    #endregion

    /*----------------------\
    |        Methods        |
    \----------------------*/

    #region Methods

    public virtual void InitState(BaseStateMachine<TStateEnum> stateMachine, TStateEnum enumValue)
    {
        _stateMachine = stateMachine;
        _enumState = enumValue;
        _transitionMap = new Dictionary<TStateEnum, Transition>();
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void UpdateState(float dT)
    {
        ChangeState();
    }

    public virtual void ChangeState()
    {
        //Ici on mettra les conditions et tout ce qui concerne les changements de state
    }

    #endregion

}
