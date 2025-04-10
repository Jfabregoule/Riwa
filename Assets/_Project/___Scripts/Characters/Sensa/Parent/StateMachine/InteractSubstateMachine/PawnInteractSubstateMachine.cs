using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PawnInteractSubstateMachine<TStateEnum> : BaseStateMachine<EnumInteract, PawnInteractBaseSubstate<TStateEnum>>
    where TStateEnum : Enum
{
    protected const string STANDBY_NAME = "StandBy";
    protected const string CHECK_NAME = "Check";
    protected const string ACTION_NAME = "Action";
    protected const string MOVE_NAME = "Move";

    private GameObject currentObjectInteract;

    public PawnInteractSubstateMachine()
    {
        //animation map et transition pas obligatoires

        _transition = new BaseTransitions(); //Dans l'idéal créer une class transition par state machine 
        States = new();
        _animationMap = new();
    }

    public GameObject CurrentObjectInteract { get => currentObjectInteract; set => currentObjectInteract = value; }

    public virtual void InitStateMachine(APawn<TStateEnum> character)
    {
        base.InitStateMachine();
    }

}
