public class CinematicStateCharacter : BaseStateCharacter<EnumStateCharacter>
{
    /// <summary>
    /// State ou le joueur aura les inputs bloqués pour les cinématiques
    /// pourra surement prendre en paramètre un sequencer, et jouera ce sequncer dans ce state
    /// </summary>

    public override void InitState(StateMachinePawn<EnumStateCharacter, BaseStatePawn<EnumStateCharacter>> stateMachine, EnumStateCharacter enumValue, APawn<EnumStateCharacter> character)
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
}
