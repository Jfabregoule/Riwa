using UnityEngine;

public class CharacterActionStateInteract : PawnActionStateInteract<EnumStateCharacter>
{
    /// <summary>
    /// State qui va trigger l'animation de sensa qui intéragit
    /// </summary>

    float _animClock;

    new private ACharacter _character;

    public override void InitState(PawnInteractSubstateMachine<EnumStateCharacter> stateMachine, EnumInteract enumValue, APawn<EnumStateCharacter> character)
    {
        base.InitState(stateMachine, enumValue, character);
        _character = (ACharacter)character;
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
        
        _subStateMachine.CurrentObjectInteract.GetComponent<IInteractable>().Interactable();
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
