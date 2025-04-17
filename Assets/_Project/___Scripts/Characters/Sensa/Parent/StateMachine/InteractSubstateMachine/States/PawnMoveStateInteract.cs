using System;
using UnityEngine;

public class PawnMoveStateInteract<TStateEnum> : PawnInteractBaseSubstate<TStateEnum>
    where TStateEnum : Enum
{
    protected bool _endInteract;

    public override void InitState(PawnInteractSubstateMachine<TStateEnum> stateMachine, EnumInteract enumValue, APawn<TStateEnum> character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();

        float radiusOffset = _subStateMachine.CurrentObjectInteract.GetComponent<IInteractable>().OffsetRadius * _subStateMachine.CurrentObjectInteract.transform.localScale.x;

        if (radiusOffset < 0)
        {
            InteractEndOfPath();
            return;
        }

        _character.OnMoveToFinished += InteractEndOfPath;
        _character.InputManager.OnInteractEnd += EndInteract;

        _endInteract = false;

        if (_subStateMachine.CurrentObjectInteract == null)
        {
            ChangeStateToIdle();
            return;
        }

        float radius = _subStateMachine.CurrentObjectInteract.GetComponent<IInteractable>().OffsetRadius;

        //On va regarder quel point de grab est le plus proche de sensa

        Vector3[] objectPoints;
        objectPoints = new Vector3[4];

        objectPoints[0] = new Vector3(1, 0, 0) * radius;
        objectPoints[1] = new Vector3(0, 0, 1) * radius;
        objectPoints[2] = new Vector3(-1, 0, 0) * radius;
        objectPoints[3] = new Vector3(0, 0, -1) * radius;

        int index = 0;
        Vector3 objPos = _subStateMachine.CurrentObjectInteract.transform.position;
        float distance = Vector3.Distance(_character.transform.position, objPos + objectPoints[index]);

        for (int i = 1; i < objectPoints.Length; i++)
        {
            if (Vector3.Distance(_character.transform.position, objPos + objectPoints[i]) < distance)
            {
                distance = Vector3.Distance(_character.transform.position, objPos + objectPoints[i]);
                index = i;
            }
        }

        _character.MoveTo(objectPoints[index] + objPos, objPos, radius > 0);
    }

    public override void ExitState()
    {
        base.ExitState();

        _character.OnMoveToFinished -= InteractEndOfPath;
        _character.InputManager.OnInteractEnd -= EndInteract;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        CheckChangeState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();
    }

    public virtual void InteractEndOfPath() { }

    protected void EndInteract()
    {
        _endInteract = true;
    }

    protected virtual void ChangeStateToIdle() { }
    protected virtual void ChangeStateToSoul() { }
    protected virtual void SetHoldingObject() { }

}
