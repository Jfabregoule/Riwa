using UnityEngine;

public class CharacterMoveStateInteract : PawnMoveStateInteract<EnumStateCharacter>
{

    new private ACharacter _character;
    private bool _endInteract;

    public override void InitState(PawnInteractSubstateMachine<EnumStateCharacter> stateMachine, EnumInteract enumValue, APawn<EnumStateCharacter> character)
    {
        base.InitState(stateMachine, enumValue, character);
        _character = (ACharacter)character;
    }

    public override void EnterState()
    {
        base.EnterState();

        _character.OnMoveToFinished += InteractEndOfPath;
        _character.InputManager.OnInteractEnd += EndInteract;

        _endInteract = false;

        if (_stateMachine.CurrentObjectInteract == null)
        {
            _character.StateMachine.ChangeState(_character.StateMachine.States[EnumStateCharacter.Idle]);
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
        if (_stateMachine.CurrentObjectInteract.TryGetComponent(out IHoldable holdable))
        {
            if (_endInteract)
            {
                _character.StateMachine.ChangeState(_character.StateMachine.States[EnumStateCharacter.Idle]);
            }
            else
            {
                _character.SetHoldingObject(_stateMachine.CurrentObjectInteract);
                _character.StateMachine.ChangeState(_character.StateMachine.States[EnumStateCharacter.Holding]);
            }
            return;
        }

        if (_stateMachine.CurrentObjectInteract.TryGetComponent(out ITreeStump stump))
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
