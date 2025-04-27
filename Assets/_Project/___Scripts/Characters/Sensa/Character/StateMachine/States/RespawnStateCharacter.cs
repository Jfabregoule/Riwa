using UnityEngine;

public class RespawnStateCharacter : BaseStateCharacter<EnumStateCharacter>
{
    public override void InitState(StateMachinePawn<EnumStateCharacter, BaseStatePawn<EnumStateCharacter>> stateMachine, EnumStateCharacter enumValue, APawn<EnumStateCharacter> character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();

        ACharacter chara = (ACharacter)_character;

        _character.Rb.velocity = Vector3.zero;

        _character.transform.position = chara.RespawnPosition;
        _character.transform.localEulerAngles = chara.RespawnRotation;

        chara.Animator.SetBool("Land", true);

        ACharacter character = (ACharacter)_character;
        character.InvokeFallStun();
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

        //_character.Animator.SetBool("Land", true);
        //_stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Idle]);
    }
}
