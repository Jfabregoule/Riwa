public class HoldingStateMachine : BaseStateMachine<EnumHolding, HoldingBaseState>
{
    private const string IDLEHOLDING_NAME = "IdleHolding";
    private const string MOVE_NAME = "MoveHolding";
    private const string ROTATE_NAME = "Rotate";
    private const string STANDBY_NAME = "StandByHolding";

    public HoldingStateMachine()
    {
        //animation map et transition pas obligatoires

        _transition = new BaseTransitions(); //Dans l'idéal créer une class transition par state machine 
        States = new();
        _animationMap = new();
    }

    public void InitStateMachine(ACharacter character)
    {
        States[EnumHolding.IdleHolding] = new IdleHoldingStateHolding();
        States[EnumHolding.IdleHolding].InitState(this, EnumHolding.IdleHolding, character);
        _animationMap[EnumHolding.IdleHolding] = IDLEHOLDING_NAME;

        States[EnumHolding.Move] = new MoveStateHolding();
        States[EnumHolding.Move].InitState(this, EnumHolding.Move, character);
        _animationMap[EnumHolding.Move] = MOVE_NAME;

        States[EnumHolding.Rotate] = new RotateStateHolding();
        States[EnumHolding.Rotate].InitState(this, EnumHolding.Rotate, character);
        _animationMap[EnumHolding.Rotate] = ROTATE_NAME;

        States[EnumHolding.StandBy] = new StandByHoldingState();
        States[EnumHolding.StandBy].InitState(this, EnumHolding.StandBy, character);
        _animationMap[EnumHolding.StandBy] = STANDBY_NAME;

    }
}
