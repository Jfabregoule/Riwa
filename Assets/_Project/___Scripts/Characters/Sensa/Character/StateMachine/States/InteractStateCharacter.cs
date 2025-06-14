public class InteractStateCharacter : PawnInteractState<EnumStateCharacter>
{
    private float _currentOffset;

    public ACharacter KARA { get => (ACharacter)_character; }

    public override void InitState(StateMachinePawn<EnumStateCharacter, BaseStatePawn<EnumStateCharacter>> stateMachine, EnumStateCharacter enumValue, APawn<EnumStateCharacter> character)
    {
        base.InitState(stateMachine, enumValue, character);

        _subStateMachine = new CharacterInteractSubstateMachine();
        _subStateMachine.InitStateMachine(character);
        _subStateMachine.InitState(_subStateMachine.States[EnumInteract.StandBy]);

    }

    public override void EnterState()
    {
        base.EnterState();

        _subStateMachine.ChangeState(_subStateMachine.States[EnumInteract.Check]);
    }

    public override void ExitState()
    {
        base.ExitState();

        _subStateMachine.ChangeState(_subStateMachine.States[EnumInteract.StandBy]);
        _character?.InteractEnd();
    }
    
    public override void UpdateState()
    {
        base.UpdateState();
        _subStateMachine.StateMachineUpdate();

    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

    }    
}
