using UnityEngine;

public class CharacterActionStateInteract : PawnActionStateInteract<EnumStateCharacter>
{
    /// <summary>
    /// State qui va trigger l'animation de sensa qui intéragit
    /// </summary>

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
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        if(_animClock > _animationTime) //Temps d'animation
        { 
            ACharacter chara = (ACharacter)_character;
            chara.StateMachine.ChangeState(chara.StateMachine.States[EnumStateCharacter.Idle]);
        }
    }
}
