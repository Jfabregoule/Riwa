using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionStateInteract : InteractBaseState
{
    /// <summary>
    /// State qui va trigger l'animation de sensa qui intéragit
    /// </summary>

    float _animClock;

    public override void InitState(InteractStateMachine stateMachine, EnumInteract enumValue, ACharacter character)
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
        _stateMachine.CurrentObjectInteract.GetComponent<IInteractable>().Interactable();
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
            _character.FsmCharacter.ChangeState(_character.FsmCharacter.States[EnumStateCharacter.Idle]);
        }
    }
}
