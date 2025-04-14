using System;
using System.Collections.Generic;
using UnityEngine;

public enum EnumInteract
{
    Check,
    Move,
    Action,
    StandBy
}

public class PawnInteractBaseSubstate<TStateEnum> : BaseState<EnumInteract>
    where TStateEnum : Enum //Enum pour soul et character
{
    protected APawn<TStateEnum> _character;
    protected PawnInteractSubstateMachine<TStateEnum> _subStateMachine;

    public virtual void InitState(PawnInteractSubstateMachine<TStateEnum> stateMachine, EnumInteract enumValue, APawn<TStateEnum> character)
    {
        base.InitState(enumValue);

        _subStateMachine = stateMachine;
        _character = character;

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

        //CheckChangeState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();
    }


    public GameObject SortObjects(Vector3 playerPos, List<GameObject> objs)
    {
        ///<summary>
        /// Renvoie l'object interactable le plus proche du player
        /// </summary>

        GameObject closestObj = objs[0];

        float distance = Vector3.Distance(closestObj.transform.position, playerPos);

        for (int i = 1; i < objs.Count; i++)
        {
            if (Vector3.Distance(objs[i].transform.position, playerPos) < distance)
            {
                closestObj = objs[i];
                distance = Vector3.Distance(closestObj.transform.position, playerPos);
            }
        }

        return closestObj;

    }

}
