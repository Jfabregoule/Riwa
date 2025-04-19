using UnityEngine;

public class RespawnStateCharacter : BaseStateCharacter<EnumStateCharacter>
{
    new private ACharacter _character;

    public override void InitState(StateMachinePawn<EnumStateCharacter, BaseStatePawn<EnumStateCharacter>> stateMachine, EnumStateCharacter enumValue, APawn<EnumStateCharacter> character)
    {
        base.InitState(stateMachine, enumValue, character);

        _character = (ACharacter)character;

    }

    public override void EnterState()
    {
        base.EnterState();

        _character.Rb.velocity = Vector3.zero;

        _character.transform.position = _character.RespawnPosition;
        _character.transform.localEulerAngles = _character.RespawnRotation;

        //_character.OnRespawn?.Invoke():
        //_character.OnRespawn?.Invoke();
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

        _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Idle]);
    }
}
