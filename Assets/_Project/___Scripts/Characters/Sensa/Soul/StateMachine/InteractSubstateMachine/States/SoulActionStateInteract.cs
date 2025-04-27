using UnityEngine;

public class SoulActionStateInteract : PawnActionStateInteract<EnumStateSoul>
{
    public override void InitState(PawnInteractSubstateMachine<EnumStateSoul> stateMachine, EnumInteract enumValue, APawn<EnumStateSoul> character)
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

        _subStateMachine.CurrentObjectInteract.GetComponent<IInteractableBase>().Interact();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        ASoul chara = (ASoul)_character;
        chara.StateMachine.ChangeState(chara.StateMachine.States[EnumStateSoul.Idle]);
        
    }
}
