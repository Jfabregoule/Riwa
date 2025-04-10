using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteractSubstateMachine : PawnInteractSubstateMachine<EnumStateCharacter>
{
    public override void InitStateMachine(APawn<EnumStateCharacter> character)
    {
        base.InitStateMachine();

        States[EnumInteract.StandBy] = new CharacterStandByStateInteract();
        States[EnumInteract.StandBy].InitState(this, EnumInteract.StandBy, character);
        _animationMap[EnumInteract.StandBy] = STANDBY_NAME;

        States[EnumInteract.Check] = new CharacterCheckStateInteract();
        States[EnumInteract.Check].InitState(this, EnumInteract.Check, character);
        _animationMap[EnumInteract.Check] = CHECK_NAME;

        States[EnumInteract.Action] = new CharacterActionStateInteract();
        States[EnumInteract.Action].InitState(this, EnumInteract.Action, character);
        _animationMap[EnumInteract.Action] = ACTION_NAME;

        States[EnumInteract.Move] = new CharacterMoveStateInteract();
        States[EnumInteract.Move].InitState(this, EnumInteract.Move, character);
        _animationMap[EnumInteract.Move] = MOVE_NAME;

    }

}
