using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActionStateInteract : PawnActionStateInteract<EnumStateCharacter>
{
    /// <summary>
    /// State qui va trigger l'animation de sensa qui intéragit
    /// </summary>

    float _animClock;

    public override void InitState(PawnInteractSubstateMachine<EnumStateCharacter> stateMachine, EnumInteract enumValue, APawn<EnumStateCharacter> character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
        //NaTTAN
        //_stateMachine.CurrentObjectInteract.GetComponent<IInteractable>().Interactable();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _animClock += Time.deltaTime;

    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        if(_animClock > 1) //Temps d'animation
        {
            _character.StateMachine.ChangeState(_character.StateMachine.States[EnumStateCharacter.Idle]);
        }
    }
}
