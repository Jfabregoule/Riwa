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

    private ChangeTempoStateMachine _subStateMachine;

    public override void InitState(StateMachineCharacter stateMachine, EnumStateCharacter enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);

        _changeTime = GameObject.Find("Sphere").GetComponent<ChangeTime>();

        ///////////////////

        _subStateMachine = new ChangeTempoStateMachine();
        _subStateMachine.InitStateMachine(_character);

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
