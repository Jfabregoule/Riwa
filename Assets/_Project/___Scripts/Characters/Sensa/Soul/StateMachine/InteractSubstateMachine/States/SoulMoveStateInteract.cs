using UnityEngine;

public class SoulMoveStateInteract : PawnMoveStateInteract<EnumStateSoul>
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
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();
    }

    public override void InteractEndOfPath()
    {
        if (_subStateMachine.CurrentObjectInteract.TryGetComponent(out ITreeStump stump))
        {
            ChangeStateToIdle();

            SoulInteractSubstateMachine stateMachine = (SoulInteractSubstateMachine)_subStateMachine;
            stateMachine.Player.StateMachine.ChangeState(stateMachine.Player.StateMachine.States[EnumStateCharacter.Idle]);
            return;
        }

        if (_subStateMachine.CurrentObjectInteract.TryGetComponent(out IInteractableSoul bud))
        {
            bud.InteractableSoul(); 
            bud.Interact();
            ChangeStateToIdle();
            return;
        }

        //Default
        _subStateMachine.ChangeState(_subStateMachine.States[EnumInteract.Action]);

    }

    protected override void ChangeStateToIdle()
    {
        ASoul chara = (ASoul)_character;
        chara.StateMachine.ChangeState(chara.StateMachine.States[EnumStateSoul.Idle]);
    }
    protected override void ChangeStateToSoul() {
        
    }
    protected override void SetHoldingObject() { }

}