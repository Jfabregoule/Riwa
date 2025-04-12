using UnityEngine;

public class SoulMoveStateInteract : PawnMoveStateInteract<EnumStateSoul>
{
    new private ASoul _character;

    public override void InitState(PawnInteractSubstateMachine<EnumStateSoul> stateMachine, EnumInteract enumValue, APawn<EnumStateSoul> character)
    {
        base.InitState(stateMachine, enumValue, character);
        _character = (ASoul)character;
    }

    public override void EnterState()
    {
        base.EnterState();

        _character.OnMoveToFinished += InteractEndOfPath;

        if (_stateMachine.CurrentObjectInteract == null)
        {
            _character.StateMachine.ChangeState(_character.StateMachine.States[EnumStateSoul.Idle]);
            return;
        }

        float radius = _stateMachine.CurrentObjectInteract.GetComponent<IInteractable>().OffsetRadius;

        //On va regarder quel point de grab est le plus proche de sensa

        Vector3[] objectPoints;
        objectPoints = new Vector3[4];

        objectPoints[0] = new Vector3(1, 0, 0) * radius;
        objectPoints[1] = new Vector3(0, 0, 1) * radius;
        objectPoints[2] = new Vector3(-1, 0, 0) * radius;
        objectPoints[3] = new Vector3(0, 0, -1) * radius;

        int index = 0;
        Vector3 objPos = _stateMachine.CurrentObjectInteract.transform.position;
        float distance = Vector3.Distance(_character.transform.position, objPos + objectPoints[index]);

        for (int i = 1; i < objectPoints.Length; i++)
        {
            if (Vector3.Distance(_character.transform.position, objPos + objectPoints[i]) < distance)
            {
                distance = Vector3.Distance(_character.transform.position, objPos + objectPoints[i]);
                index = i;
            }
        }

        _character.MoveTo(objectPoints[index] + objPos, objPos);
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
    }

    public void InteractEndOfPath()
    {
        if (_stateMachine.CurrentObjectInteract.TryGetComponent(out ITreeStump stump))
        {
            _character.StateMachine.ChangeState(_character.StateMachine.States[EnumStateSoul.Idle]);
            _character.transform.parent.GetComponent<ACharacter>().StateMachine.ChangeState(_character.transform.parent.GetComponent<ACharacter>().StateMachine.States[EnumStateCharacter.Idle]); //horrible à changer
            return;
        }

        //Default
        _stateMachine.ChangeState(_stateMachine.States[EnumInteract.Action]);

    }

}