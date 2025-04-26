public enum EnumHolding
{
    IdleHolding, 
    Move,
    Rotate
}

public class HoldingBaseState : BaseState<EnumHolding>
{
    new protected HoldingStateMachine _stateMachine;
    protected ACharacter _character;
    protected CameraHandler _cam;

    public virtual void InitState(HoldingStateMachine stateMachine, EnumHolding enumValue, ACharacter character)
    {
        base.InitState(enumValue);

        _stateMachine = stateMachine;
        _character = character;

        _cam = GameManager.Instance.CameraHandler;

    }

    public override void EnterState()
    {
        base.EnterState();
        if (_character.Animator != null && Helpers.HasParameter(_stateMachine.AnimationMap[_enumState], _character.Animator))
        {
            _character.Animator.SetTrigger(_stateMachine.AnimationMap[_enumState]); //Lorsque je rentre dans un state, je trigger l'animation à jouer, si l'animator est bien fait, tout est clean  
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        if(_character.Animator != null && Helpers.HasParameter(_stateMachine.AnimationMap[_enumState], _character.Animator))
        {
            _character.Animator.ResetTrigger(_stateMachine.AnimationMap[_enumState]); //Lorsque je rentre dans un state, je trigger l'animation   jouer, si l'animator est bien fait, tout est clean  
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();
    }
}
