using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTempoStateMachine : BaseStateMachine<EnumChangeTempo, ChangeTempoBaseState>
{
    public ChangeTempoStateMachine()
    {
        //animation map et transition pas obligatoires

        _transition = new BaseTransitions(); //Dans l'idéal créer une class transition par state machine 
        States = new Dictionary<EnumChangeTempo, ChangeTempoBaseState>();
        _animationMap = new Dictionary<EnumChangeTempo, string>(); 
    }

    public void InitStateMachine(ACharacter character)
    {
        //ICI CREER ET INIT TOUS LES STATES
    }

}
