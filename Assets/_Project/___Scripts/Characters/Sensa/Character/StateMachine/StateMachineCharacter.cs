public class StateMachineCharacter : StateMachinePawn<EnumStateCharacter, BaseStatePawn<EnumStateCharacter>>
{

    private ChangeTempoStateMachine _changeTempoSubstateMachine;

    #region StateNameAnimation

    private const string IDLE_NAME          = "Idle";
    private const string MOVE_NAME          = "Move";
    private const string INTERACT_NAME      = "Interact";
    private const string CHANGETEMPO_NAME   = "ChangeTempo";
    private const string CINEMATIC_NAME     = "Cinematic";
    private const string HOLDING_NAME       = "Holding";
    private const string SOUL_NAME          = "Soul";
    private const string WAIT_NAME          = "Wait";
    private const string FALL_NAME          = "Fall";
    private const string RESPAWN_NAME       = "Respawn";

    #endregion

    public StateMachineCharacter() { 

        _transition = new();
        States = new();
        _animationMap = new();
    }

    public void InitStateMachine(ACharacter character)
    {
        base.InitStateMachine();

        States[EnumStateCharacter.Idle] = new IdleStateCharacter();
        States[EnumStateCharacter.Idle].InitState(this, EnumStateCharacter.Idle, character);
        _animationMap[EnumStateCharacter.Idle] = IDLE_NAME;

        States[EnumStateCharacter.Move] = new MoveStateCharacter();
        States[EnumStateCharacter.Move].InitState(this, EnumStateCharacter.Move, character);
        _animationMap[EnumStateCharacter.Move] = MOVE_NAME;

        States[EnumStateCharacter.Interact] = new InteractStateCharacter();
        States[EnumStateCharacter.Interact].InitState(this, EnumStateCharacter.Interact, character);
        _animationMap[EnumStateCharacter.Interact] = INTERACT_NAME;

        States[EnumStateCharacter.ChangeTempo] = new ChangeTempoStateCharacter();
        States[EnumStateCharacter.ChangeTempo].InitState(this, EnumStateCharacter.ChangeTempo, character);
        _animationMap[EnumStateCharacter.ChangeTempo] = CHANGETEMPO_NAME;

        States[EnumStateCharacter.Soul] = new SoulStateCharacter();
        States[EnumStateCharacter.Soul].InitState(this, EnumStateCharacter.Soul, character);
        _animationMap[EnumStateCharacter.Soul] = SOUL_NAME;

        States[EnumStateCharacter.Holding] = new HoldingStateCharacter();
        States[EnumStateCharacter.Holding].InitState(this, EnumStateCharacter.Holding, character);
        _animationMap[EnumStateCharacter.Holding] = HOLDING_NAME;

        States[EnumStateCharacter.Cinematic] = new CinematicStateCharacter();
        States[EnumStateCharacter.Cinematic].InitState(this, EnumStateCharacter.Cinematic, character);
        _animationMap[EnumStateCharacter.Cinematic] = CINEMATIC_NAME;

        States[EnumStateCharacter.Wait] = new WaitStateCharacter();
        States[EnumStateCharacter.Wait].InitState(this, EnumStateCharacter.Wait, character);
        _animationMap[EnumStateCharacter.Wait] = WAIT_NAME;

        States[EnumStateCharacter.Fall] = new FallStateCharacter();
        States[EnumStateCharacter.Fall].InitState(this, EnumStateCharacter.Fall, character);
        _animationMap[EnumStateCharacter.Fall] = FALL_NAME;

        States[EnumStateCharacter.Respawn] = new RespawnStateCharacter();
        States[EnumStateCharacter.Respawn].InitState(this, EnumStateCharacter.Respawn, character);
        _animationMap[EnumStateCharacter.Respawn] = RESPAWN_NAME;

        ChangeTempoStateCharacter temp = (ChangeTempoStateCharacter)States[EnumStateCharacter.ChangeTempo];
        _changeTempoSubstateMachine = temp.SubStateMachine;

    }

    public override void StateMachineUpdate()
    {
        base.StateMachineUpdate();

        _changeTempoSubstateMachine.StateMachineUpdate();

    }

    public override void StateMachineFixedUpdate()
    {
        base.StateMachineFixedUpdate();

        _changeTempoSubstateMachine.StateMachineFixedUpdate();

    }


    public override void GoToIdle()
    {
        ChangeState(States[EnumStateCharacter.Idle]);
    }

}
