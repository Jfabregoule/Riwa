using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoulStateCharacter : BaseStateCharacter
{
    /// <summary>
    /// State dans lequel le joueur va changer de temporalité
    /// L'entrée vers ce state n'a pas encore été définit
    /// On y fera le check de si le voyage temporel est possible ou non
    /// </summary>

    public override void InitState(StateMachineCharacter stateMachine, EnumStateCharacter enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();
        _character.Soul.transform.position = _character.transform.position;
        _character.Soul.transform.rotation = _character.transform.rotation;
        _character.SoulLinkVFX.Play();
        _character.Soul.SetActive(true);
    }

    public override void ExitState()
    {
        base.ExitState();
        _character.SoulLinkVFX.Stop();
        _character.Soul.SetActive(false);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Vector3 VFXOrientation = _character.Soul.transform.position - _character.transform.position;

        _character.SoulLinkVFX.transform.forward = VFXOrientation.normalized;
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
