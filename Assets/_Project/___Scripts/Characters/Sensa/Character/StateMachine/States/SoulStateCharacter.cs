using UnityEngine;

public class SoulStateCharacter : BaseStateCharacter<EnumStateCharacter>
{
    /// <summary>
    /// State dans lequel le joueur va changer de temporalité
    /// L'entrée vers ce state n'a pas encore été définit
    /// On y fera le check de si le voyage temporel est possible ou non
    /// </summary>
    new private ACharacter _character;

    public override void InitState(StateMachinePawn<EnumStateCharacter, BaseStatePawn<EnumStateCharacter>> stateMachine, EnumStateCharacter enumValue, APawn<EnumStateCharacter> character)
    {
        base.InitState(stateMachine, enumValue, character);
        _character = (ACharacter)character;
    }
        
    public override void EnterState()
    {
        base.EnterState();

        _character.Soul.transform.position = _character.transform.position;
        _character.Soul.transform.rotation = _character.transform.rotation;
        _character.Soul.SetActive(true);
        _character.IsInSoul = true;
        _character.Soul.transform.parent = null;

        RiwaSoundSystem.Instance.PlaySoundFXClipByKey("Soul Exit", _character.transform.position);

        ASoul soul = _character.Soul.GetComponent<ASoul>();
        soul.StateMachine.ChangeState(soul.StateMachine.States[EnumStateSoul.Idle]);

        GameManager.Instance.CameraHandler.VirtualCamera.LookAt = _character.Soul.transform;
    }

    public override void ExitState()
    {
        base.ExitState();
        _character.Soul.SetActive(false);
        _character.IsInSoul = false;
        _character.Soul.transform.parent = _character.transform;

        RiwaSoundSystem.Instance.PlaySoundFXClipByKey("Soul Enter", _character.transform.position);

        ASoul soul = _character.Soul.GetComponent<ASoul>();
        soul.StateMachine.ChangeState(soul.StateMachine.States[EnumStateSoul.Disable]);

        GameManager.Instance.CameraHandler.ResetLookAt();

    }

    public override void UpdateState()
    {
        base.UpdateState();

    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        if (_character.IsInSoul == false)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Idle]);
            return;
        }
    }
}
