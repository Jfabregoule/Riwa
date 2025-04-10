using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteractBaseSubstate : PawnInteractBaseSubstate<EnumStateCharacter>
{
    new protected InteractStateMachine<EnumStateCharacter> _stateMachine;
    protected APawn<EnumStateCharacter> _character;

    public virtual void InitState(InteractStateMachine<EnumStateCharacter> stateMachine, EnumInteract enumValue, APawn<EnumStateCharacter> character)
    {
        base.InitState(enumValue);

        _stateMachine = stateMachine;
        _character = character;
    }

}
