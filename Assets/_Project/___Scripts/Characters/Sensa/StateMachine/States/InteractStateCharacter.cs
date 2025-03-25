using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractStateCharacter : BaseStateCharacter
{
    new public void InitState(FSMCharacter stateMachine, Character character)
    {
        base.InitState(stateMachine, character);
    }

    new public void EnterState()
    {
        base.EnterState();

        //Logique interraction

    }

    new public void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        //Idéalement bloquer le state 0.5secondes pour la fluidité
        ChangeState();
    }

    public override void ChangeState()
    {
        
    }
}
