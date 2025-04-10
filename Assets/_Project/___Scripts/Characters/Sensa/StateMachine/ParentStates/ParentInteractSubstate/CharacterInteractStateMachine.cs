using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteractStateMachine : PawnInteractSubstateMachine<EnumStateCharacter>
{
    protected GameObject _currentObjectInteract;

    public GameObject CurrentObjectInteract { get; set; }

    public void InitStateMachine(APawn<EnumStateCharacter> character)
    {
        //States[EnumInteract.StandBy] = new StandByStateInteract<TStateEnum>();
        //States[EnumInteract.StandBy].InitState(this, EnumInteract.StandBy, character);
        //_animationMap[EnumInteract.StandBy] = STANDBY_NAME;

        //States[EnumInteract.Check] = new CheckStateInteract<TStateEnum>();
        //States[EnumInteract.Check].InitState(this, EnumInteract.Check, character);
        //_animationMap[EnumInteract.Check] = CHECK_NAME;

        //States[EnumInteract.Action] = new ActionStateInteract<TStateEnum>();
        //States[EnumInteract.Action].InitState(this, EnumInteract.Action, character);
        //_animationMap[EnumInteract.Action] = ACTION_NAME;

        //States[EnumInteract.Move] = new MoveStateInteract<TStateEnum>();
        //States[EnumInteract.Move].InitState(this, EnumInteract.Move, character);
        //_animationMap[EnumInteract.Move] = MOVE_NAME;

    }

}
