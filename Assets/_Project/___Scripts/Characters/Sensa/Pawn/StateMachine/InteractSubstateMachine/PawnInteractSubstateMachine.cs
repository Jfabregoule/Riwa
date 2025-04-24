using System;
using UnityEngine;

public abstract class PawnInteractSubstateMachine<TStateEnum> : BaseStateMachine<EnumInteract, PawnInteractBaseSubstate<TStateEnum>>
    where TStateEnum : Enum
{
    protected const string STANDBY_NAME = "StandBy";
    protected const string CHECK_NAME = "Check";
    protected const string ACTION_NAME = "ActionInteract";
    protected const string MOVE_NAME = "MoveInteract";

    protected GameObject _currentObjectInteract;

    public PawnInteractSubstateMachine()
    {
        //animation map et transition pas obligatoires

        _transition = new BaseTransitions(); //Dans l'idéal créer une class transition par state machine 
        States = new();
        _animationMap = new();
    }

    public GameObject CurrentObjectInteract { get => _currentObjectInteract; set => _currentObjectInteract = value; }

    public virtual void InitStateMachine(APawn<TStateEnum> character)
    {
        base.InitStateMachine();
    }

}
