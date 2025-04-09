using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractStateMachine<TStateEnum> : BaseStateMachine<EnumInteract, InteractBaseState<TStateEnum>>
    where TStateEnum : Enum
{
    private const string STANDBY_NAME = "StandBy";
    private const string CHECK_NAME = "Check";
    private const string ACTION_NAME = "Action";
    private const string MOVE_NAME = "Move";

    private GameObject _currentObjectInteract;

    public InteractStateMachine()
    {
        //animation map et transition pas obligatoires

        _transition = new BaseTransitions(); //Dans l'idéal créer une class transition par state machine 
        States = new();
        _animationMap = new();
    }

    public GameObject CurrentObjectInteract { get; set; }

    public void InitStateMachine(APawn<TStateEnum> character)
    {
        States[EnumInteract.StandBy] = new StandByStateInteract<TStateEnum>();
        States[EnumInteract.StandBy].InitState(this, EnumInteract.StandBy, character);
        _animationMap[EnumInteract.StandBy] = STANDBY_NAME;

        States[EnumInteract.Check] = new CheckStateInteract<TStateEnum>();
        States[EnumInteract.Check].InitState(this, EnumInteract.Check, character);
        _animationMap[EnumInteract.Check] = CHECK_NAME;

        States[EnumInteract.Action] = new ActionStateInteract<TStateEnum>();
        States[EnumInteract.Action].InitState(this, EnumInteract.Action, character);
        _animationMap[EnumInteract.Action] = ACTION_NAME;

        States[EnumInteract.Move] = new MoveStateInteract<TStateEnum>();
        States[EnumInteract.Move].InitState(this, EnumInteract.Move, character);
        _animationMap[EnumInteract.Move] = MOVE_NAME;

    }
}
