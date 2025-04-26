
using UnityEngine;

public class RotateStateHolding : HoldingBaseState
{
    private int _sens;

    private IRotatable _rotatable;
    public int Sens { get => _sens; set => _sens = value; }
    public override void InitState(HoldingStateMachine stateMachine, EnumHolding enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();

        if(_character.HoldingObject.TryGetComponent(out IRotatable rotatable))
        {
            _rotatable = rotatable;
            rotatable.Rotate(Sens);
            rotatable.OnRotateFinished += Finish;
        }

        if(Sens == 1)
        {
            _character.Animator.SetBool("Sens", false);
        }
        else
        {
            _character.Animator.SetBool("Sens", true);
        }

    }

    public override void ExitState()
    {
        base.ExitState();

        _rotatable.OnRotateFinished -= Finish;

    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

    }

    private void Finish()
    {
        Vector2 joystickDir = _character.InputManager.GetMoveDirection();

        if (joystickDir == Vector2.zero)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumHolding.IdleHolding]);
            return;
        }

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
                //Sens = 1;
                ((RotateStateHolding)_stateMachine.States[EnumHolding.Rotate]).Sens = 1;
                _stateMachine.ChangeState(_stateMachine.States[EnumHolding.Rotate]);
            }
            else
            {
                //Rotate Gauche
                //Sens = -1;
                ((RotateStateHolding)_stateMachine.States[EnumHolding.Rotate]).Sens = -1;
                _stateMachine.ChangeState(_stateMachine.States[EnumHolding.Rotate]);

            }
        }
    }
}
