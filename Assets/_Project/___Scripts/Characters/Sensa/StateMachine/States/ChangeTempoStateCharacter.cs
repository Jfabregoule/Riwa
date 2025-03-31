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

    public override void InitState(FSMCharacter stateMachine, Character character)
    {
        base.InitState(stateMachine, character);

        _enumState = EnumStateCharacter.ChangeTempo;

    }

    public override void EnterState()
    {
        base.EnterState();

        Debug.Log("Je change de temps");

    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ChangeState()
    {
        base.ChangeState();

        _stateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Idle]);
    }
}
