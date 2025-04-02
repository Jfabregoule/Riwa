using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTempoStateCharacter : BaseStateCharacter
{
    /// <summary>
    /// State dans lequel le joueur va changer de temporalité
    /// L'entrée vers ce state n'a pas encore été définit
    /// On y fera le check de si le voyage temporel est possible ou non
    /// </summary>

    private ChangeTime _changeTime;

    public override void InitState(FSMCharacter stateMachine, Character character)
    {
        base.InitState(stateMachine, character);

        _enumState = EnumStateCharacter.ChangeTempo;

        _changeTime = GameObject.Find("Sphere").GetComponent<ChangeTime>(); 

    }

    public override void EnterState()
    {
        base.EnterState();

        _changeTime.isActivated = true;

    }

    public override void ExitState()
    {
        base.ExitState();

        _changeTime.UpdateShaders();
    }

    public override void UpdateState(float dT)
    {
        base.UpdateState(dT);
    }

    public override void ChangeState()
    {
        base.ChangeState();

        if (_changeTime.isActivated == false)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Idle]);
        }
    }
}
