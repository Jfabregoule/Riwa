using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingStateCharacter : BaseStateCharacter
{
    /// <summary>
    /// State dans lequel Sensa va tenir un gros objet
    /// 
    /// </summary>

    private HoldingStateMachine _subStateMachine;


    public override void InitState(StateMachineCharacter stateMachine, EnumStateCharacter enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);

        _subStateMachine = new HoldingStateMachine();
        _subStateMachine.InitStateMachine(_character);
        //_subStateMachine.InitState(_subStateMachine.States[EnumHolding.IdleHolding]);

    }

    public override void EnterState()
    {
        base.EnterState();

        _character.InputManager.OnInteractEnd += OnInteractEnd;
    }

    public override void ExitState()
    {
        base.ExitState();

        _character.InputManager.OnInteractEnd -= OnInteractEnd;
    }

    public override void UpdateState(float dT)
    {
        base.UpdateState(dT);
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        //A definir si on maintient appuye ou si on toggle pour retourner en idle
    }

    private void OnInteractEnd()
    {
        _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Idle]);
    }
}
