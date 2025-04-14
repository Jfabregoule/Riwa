using UnityEngine;

public class IdleHoldingStateHolding : HoldingBaseState
{
    public override void InitState(HoldingStateMachine stateMachine, EnumHolding enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);
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
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        Vector2 joystickDir = _character.InputManager.GetMoveDirection();

        if (joystickDir == Vector2.zero) return;

        //Pour calculer avec l'orientation de la caméra

        Vector3 camForward = Vector3.ProjectOnPlane(_cam.transform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.Cross(Vector3.up, camForward);

        /////////

        Vector3 inputDir = (camForward * joystickDir.y + camRight * joystickDir.x).normalized;

        Vector3 worldForward = _character.transform.forward;
        Vector3 worldRight = _character.transform.right;

        float dotForward = Vector3.Dot(worldForward, inputDir);
        float dotRight = Vector3.Dot(worldRight, inputDir);

        if (Mathf.Abs(dotForward) > Mathf.Abs(dotRight))
        {
            if (!_character.HoldingObject.TryGetComponent(out IMovable movable)) return;
            if (dotForward > 0.5f)
            {
                //Pull
                ((MoveStateHolding)_stateMachine.States[EnumHolding.Move]).Sens = 1;
                _stateMachine.ChangeState(_stateMachine.States[EnumHolding.Move]);
            } 
            else 
            {
                //Push
                ((MoveStateHolding)_stateMachine.States[EnumHolding.Move]).Sens = -1;
                _stateMachine.ChangeState(_stateMachine.States[EnumHolding.Move]);
            }
        }
        else
        {
            if (!_character.HoldingObject.TryGetComponent(out IRotatable rotatable)) return;
            if (dotRight > 0.5f)
            {
                //Rotate Droite
                ((RotateStateHolding)_stateMachine.States[EnumHolding.Rotate]).Sens = 1;
                _stateMachine.ChangeState(_stateMachine.States[EnumHolding.Rotate]);
            }
            else
            {
                //Rotate Gauche
                ((RotateStateHolding)_stateMachine.States[EnumHolding.Rotate]).Sens = -1;
                _stateMachine.ChangeState(_stateMachine.States[EnumHolding.Rotate]);
            }
        }
    }
}
