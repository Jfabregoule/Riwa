using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulInteractBaseSubstate : PawnInteractBaseSubstate<EnumStateSoul>
{
    new protected InteractStateMachine<EnumStateSoul> _stateMachine;
    protected APawn<EnumStateSoul> _character;

    public virtual void InitState(InteractStateMachine<EnumStateSoul> stateMachine, EnumInteract enumValue, APawn<EnumStateSoul> character)
    {
        base.InitState(enumValue);

        _stateMachine = stateMachine;
        _character = character;
    }

}
