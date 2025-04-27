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

        ACharacter chara = (ACharacter)_character;
        chara.OnInteractAnimation += Interact;
        
    }

    public override void ExitState()
    {
        base.ExitState();

        ACharacter chara = (ACharacter)_character;
        chara.OnInteractAnimation -= Interact;
    }

    public override void DestroyState()
    {
        base.DestroyState();

        ACharacter chara = (ACharacter)_character;
        chara.OnInteractAnimation -= Interact;
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();
    }

    public void Interact()
    {
        _subStateMachine.CurrentObjectInteract.GetComponent<IInteractableBase>().Interact();

        ACharacter chara = (ACharacter)_character;
        chara.StateMachine.ChangeState(chara.StateMachine.States[EnumStateCharacter.Idle]);
    }

}
