using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTempoStateMachine : BaseStateMachine<EnumChangeTempo, ChangeTempoBaseState>
{
    private const string STANDBY_NAME = "Standby";
    private const string CHECK_NAME = "Check";
    private const string PROCESS_NAME = "Process";
    private const string CANCEL_NAME = "Cancel";

    public ChangeTempoStateMachine()
    {
        //animation map et transition pas obligatoires

        _transition = new BaseTransitions(); //Dans l'idéal créer une class transition par state machine 
        States = new();
        _animationMap = new(); 
    }

    public void InitStateMachine(ACharacter character)
    {
        States[EnumChangeTempo.Standby] = new StandbyStateTempo();
        States[EnumChangeTempo.Standby].InitState(this, EnumChangeTempo.Standby, character);
        _animationMap[EnumChangeTempo.Standby] = STANDBY_NAME;

        States[EnumChangeTempo.Check] = new CheckStateTempo();
        States[EnumChangeTempo.Check].InitState(this, EnumChangeTempo.Check, character);
        _animationMap[EnumChangeTempo.Check] = CHECK_NAME;

        States[EnumChangeTempo.Process] = new ProcessStateTempo();
        States[EnumChangeTempo.Process].InitState(this, EnumChangeTempo.Process, character);
        _animationMap[EnumChangeTempo.Process] = PROCESS_NAME;

        States[EnumChangeTempo.Cancel] = new CancelStateTempo();
        States[EnumChangeTempo.Cancel].InitState(this, EnumChangeTempo.Cancel, character);
        _animationMap[EnumChangeTempo.Cancel] = CANCEL_NAME;
    }

}
