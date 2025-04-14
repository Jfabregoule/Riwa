using UnityEngine;

public class CharacterMoveStateInteract : PawnMoveStateInteract<EnumStateCharacter>
{
    private bool _endInteract;

    public override void InitState(PawnInteractSubstateMachine<EnumStateCharacter> stateMachine, EnumInteract enumValue, APawn<EnumStateCharacter> character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();


        Debug.Log("CharacterMoveState");


        float radiusOffset = _subStateMachine.CurrentObjectInteract.GetComponent<IInteractable>().OffsetRadius;

        if (radiusOffset < 0)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumInteract.Action]);
            return;
        }

        _character.OnMoveToFinished += InteractEndOfPath;
        _character.InputManager.OnInteractEnd += EndInteract;

        _endInteract = false;

        if (_subStateMachine.CurrentObjectInteract == null)
        {
            _character.StateMachine.ChangeState(_character.StateMachine.States[EnumStateCharacter.Idle]);
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

        _character.MoveTo(objectPoints[index] + objPos, objPos);

        //_stateMachine.CurrentObjectInteract.

        //Animation de ce state => marcher 
        //Création 4 points autour; regarder duquel sensa est la plus proche, on y déplace sensa, et on la rotate

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
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();
    }

    public void InteractEndOfPath()
    {
        if (_subStateMachine.CurrentObjectInteract.TryGetComponent(out IHoldable holdable))
        {
            if (_endInteract)
            {
                _character.StateMachine.ChangeState(_character.StateMachine.States[EnumStateCharacter.Idle]);
            }
            else
            {
                ACharacter charac = (ACharacter)_character;
                charac.SetHoldingObject(_subStateMachine.CurrentObjectInteract);
                _character.StateMachine.ChangeState(_character.StateMachine.States[EnumStateCharacter.Holding]);
            }
            return;
        }

        if (_subStateMachine.CurrentObjectInteract.TryGetComponent(out ITreeStump stump))
        {
            _character.StateMachine.ChangeState(_character.StateMachine.States[EnumStateCharacter.Soul]);
            return;
        }

        //Default
        _stateMachine.ChangeState(_stateMachine.States[EnumInteract.Action]);

    }

    private void EndInteract()
    {
        _endInteract = true;
    }

}
