public class HoldingStateCharacter : BaseStateCharacter<EnumStateCharacter>
{
    /// <summary>
    /// State dans lequel Sensa va tenir un gros objet
    /// 
    /// </summary>

    private HoldingStateMachine _subStateMachine;

    public override void InitState(StateMachinePawn<EnumStateCharacter, BaseStatePawn<EnumStateCharacter>> stateMachine, EnumStateCharacter enumValue, APawn<EnumStateCharacter> character)
    {
        base.InitState(stateMachine, enumValue, character);

        _subStateMachine = new HoldingStateMachine();
        _subStateMachine.InitStateMachine((ACharacter)character);
        _subStateMachine.InitState(_subStateMachine.States[EnumHolding.IdleHolding]);
        _character.Animator.ResetTrigger(_subStateMachine.AnimationMap[EnumHolding.IdleHolding]);
    }

    public override void EnterState()
    {
        base.EnterState();

        _subStateMachine.ChangeState(_subStateMachine.States[EnumHolding.IdleHolding]);
        _character.InputManager.OnInteractEnd += OnInteractEnd;
    }

    public override void ExitState()
    {
        base.ExitState();

        ACharacter chara = (ACharacter)_character;

        chara.SetHoldingObject(null);
        chara.InputManager.OnInteractEnd -= OnInteractEnd;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _subStateMachine.StateMachineUpdate();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        //A definir si on maintient appuye ou si on toggle pour retourner en idle
    }

    private void OnInteractEnd()
    {
        _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Idle]);
    }
}
