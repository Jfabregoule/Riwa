
public class SoulInteractSubstateMachine : PawnInteractSubstateMachine<EnumStateSoul>
{
    public ACharacter Player;

    public override void InitStateMachine(APawn<EnumStateSoul> character)
    {
        States[EnumInteract.StandBy] = new SoulStandByStateInteract();
        States[EnumInteract.StandBy].InitState(this, EnumInteract.StandBy, character);
        _animationMap[EnumInteract.StandBy] = STANDBY_NAME;

        States[EnumInteract.Check] = new SoulCheckStateInteract();
        States[EnumInteract.Check].InitState(this, EnumInteract.Check, character);
        _animationMap[EnumInteract.Check] = CHECK_NAME;

        States[EnumInteract.Action] = new SoulActionStateInteract();
        States[EnumInteract.Action].InitState(this, EnumInteract.Action, character);
        _animationMap[EnumInteract.Action] = ACTION_NAME;

        States[EnumInteract.Move] = new SoulMoveStateInteract();
        States[EnumInteract.Move].InitState(this, EnumInteract.Move, character);
        _animationMap[EnumInteract.Move] = MOVE_NAME;

        Player = GameManager.Instance.Character;

    }

}
