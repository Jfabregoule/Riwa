using UnityEngine;

public class MoveStateHolding : HoldingBaseState
{
    private int _sens;
    private IMovable _movable;

    private Joystick _joystick;

    public int Sens { get => _sens; set => _sens = value; }

    public override void InitState(HoldingStateMachine stateMachine, EnumHolding enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);

        _joystick = GameManager.Instance.Joystick;

    }

    public override void EnterState()
    {
        base.EnterState();
        if (_character.HoldingObject.TryGetComponent(out IMovable movable))
        {
            _movable = movable;
            _movable.OnMoveFinished += CanGoToIdle;

        }

    }

    public override void ExitState()
    {
        base.ExitState();
        _movable.OnMoveFinished -= CanGoToIdle;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Vector3 dir = Helpers.GetDominantDirection(_character.transform.forward);
        bool canMove = _movable.Move(Sens * dir);
        if (!canMove) {
            _stateMachine.ChangeState(_stateMachine.States[EnumHolding.IdleHolding]);
            return;
        }
        _character.transform.position += Sens * dir * _movable.MoveSpeed * Time.deltaTime;
        
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();
    }

    public void CanGoToIdle()
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
                Sens = 1;
            }
            else
            {
                //Push
                Sens = -1;
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
