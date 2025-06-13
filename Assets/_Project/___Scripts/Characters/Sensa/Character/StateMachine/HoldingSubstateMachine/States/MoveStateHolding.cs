using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MoveStateHolding : HoldingBaseState
{
    private int _sens;
    private IMovable _movable;
    
    private CapsuleCollider _capsuleCollider;
    
    public int Sens { get => _sens; set => _sens = value; }

    public override void InitState(HoldingStateMachine stateMachine, EnumHolding enumValue, ACharacter character)
    {
        base.InitState(stateMachine, enumValue, character);

        _capsuleCollider = _character.gameObject.GetComponent<CapsuleCollider>();

    }

    public override void EnterState()
    {
        base.EnterState();
        if (_character.HoldingObject.TryGetComponent(out IMovable movable))
        {
            _movable = movable;
            _movable.OnMoveFinished += CanGoToIdle;
            _movable.OnReplacePlayer += ReplacePlayer;
        }

        _character.Animator.SetFloat("HoldingSens", Sens);
        _character.Animator.SetFloat("PushSpeed", _movable.MoveSpeed);

        _character.InputManager.OnRotateLeft += OnRotateLeft;
        _character.InputManager.OnRotateRight += OnRotateRight;
        _character.InputManager.OnPush += OnPush;
        _character.InputManager.OnPull += OnPull;

        //_character.Feet.OnFall += GoToFall;
    }

    public override void ExitState()
    {
        base.ExitState();

        _movable.OnMoveFinished -= CanGoToIdle;
        _movable.OnReplacePlayer -= ReplacePlayer;

        _character.InputManager.OnRotateLeft -= OnRotateLeft;
        _character.InputManager.OnRotateRight -= OnRotateRight;
        _character.InputManager.OnPush -= OnPush;
        _character.InputManager.OnPull -= OnPull;
        //_character.Feet.OnFall -= GoToFall;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Vector3 dir = Helpers.GetDominantDirection(_character.transform.forward);

        float moveDistance = _movable.MoveDistance;
        //moveDistance *= 5;
        float radius = _capsuleCollider.radius;

        moveDistance = Mathf.Clamp(moveDistance, 0.5f, 1);

        LayerMask layerMask = GameManager.Instance.CurrentTemporality == EnumTemporality.Past ? _character.PastLayer : _character.PresentLayer;

        Vector3 center = _character.transform.position + -_character.transform.forward * (radius + moveDistance) + Vector3.up * _capsuleCollider.height * _character.transform.localScale.y / 2;
        Vector3 halfExtents = new Vector3(moveDistance, _capsuleCollider.height * _character.transform.localScale.y, radius * 2);
        Quaternion orientation = Quaternion.Euler(new Vector3(0, 90 * (Sens * dir).z, 0));

        halfExtents *= 0.4f;
        
        Collider[] colliders = Physics.OverlapBox(center, halfExtents, orientation, layerMask);

        DebugDrawBox(center, halfExtents, orientation, UnityEngine.Color.blue, 1f);

        foreach (var col in colliders)
        {
            if (col.gameObject != _character.gameObject
                && !col.gameObject.TryGetComponent<ACharacter>(out ACharacter chara)
                && col.isTrigger == false
                && Sens == -1)
            {
                CanGoToIdle();
                return;
            }
        }

        bool canMove = _movable.Move(Sens * dir);
        if (!canMove) {
            _stateMachine.ChangeState(_stateMachine.States[EnumHolding.IdleHolding]);
            return;
        }
        _character.transform.position += Sens * dir * _movable.MoveSpeed * Time.deltaTime;

    }

    void DebugDrawBox(Vector3 center, Vector3 halfExtents, Quaternion orientation, UnityEngine.Color color, float duration)
    {
        Vector3[] points = new Vector3[8];

        Vector3 right = orientation * Vector3.right;
        Vector3 up = orientation * Vector3.up;
        Vector3 forward = orientation * Vector3.forward;

        points[0] = center + right * halfExtents.x + up * halfExtents.y + forward * halfExtents.z;
        points[1] = center + right * halfExtents.x + up * halfExtents.y - forward * halfExtents.z;
        points[2] = center + right * halfExtents.x - up * halfExtents.y + forward * halfExtents.z;
        points[3] = center + right * halfExtents.x - up * halfExtents.y - forward * halfExtents.z;
        points[4] = center - right * halfExtents.x + up * halfExtents.y + forward * halfExtents.z;
        points[5] = center - right * halfExtents.x + up * halfExtents.y - forward * halfExtents.z;
        points[6] = center - right * halfExtents.x - up * halfExtents.y + forward * halfExtents.z;
        points[7] = center - right * halfExtents.x - up * halfExtents.y - forward * halfExtents.z;

        Debug.DrawLine(points[0], points[1], color, duration);
        Debug.DrawLine(points[0], points[2], color, duration);
        Debug.DrawLine(points[0], points[4], color, duration);
        Debug.DrawLine(points[1], points[3], color, duration);
        Debug.DrawLine(points[1], points[5], color, duration);
        Debug.DrawLine(points[2], points[3], color, duration);
        Debug.DrawLine(points[2], points[6], color, duration);
        Debug.DrawLine(points[3], points[7], color, duration);
        Debug.DrawLine(points[4], points[5], color, duration);
        Debug.DrawLine(points[4], points[6], color, duration);
        Debug.DrawLine(points[5], points[7], color, duration);
        Debug.DrawLine(points[6], points[7], color, duration);
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();

        if (!_character.Feet.IsGround)
        {
            GoToFall();
        }
    }

    public override void DestroyState()
    {
        base.DestroyState();

        _character.InputManager.OnRotateLeft -= OnRotateLeft;
        _character.InputManager.OnRotateRight -= OnRotateRight;
        _character.InputManager.OnPush -= OnPush;
        _character.InputManager.OnPull -= OnPull;
    }

    public void CanGoToIdle()
    {

        Vector2 joystickDir = _character.InputManager.GetMoveDirection();

        if (joystickDir == Vector2.zero)
        {
            _stateMachine.ChangeState(_stateMachine.States[EnumHolding.IdleHolding]);
            return;
        }
    }

    public void ReplacePlayer(Vector3 targetPosition)
    {
        Vector3 destination = ((MonoBehaviour)_movable).gameObject.transform.position; 
        Vector3 playerPosition = destination + targetPosition * -Sens;
        _character.transform.position = new Vector3(playerPosition.x, _character.transform.position.y, playerPosition.z);
    }

    public void GoToIdle()
    {
        _character.StateMachine.ChangeState(_character.StateMachine.States[EnumStateCharacter.Idle]);
    }

    public void GoToFall()
    {
        _character.StateMachine.ChangeState(_character.StateMachine.States[EnumStateCharacter.Fall]);
    }

    private void OnRotateLeft()
    {
        ((RotateStateHolding)_stateMachine.States[EnumHolding.Rotate]).Sens = 1;
        _stateMachine.ChangeState(_stateMachine.States[EnumHolding.Rotate]);
    }

    private void OnRotateRight()
    {
        ((RotateStateHolding)_stateMachine.States[EnumHolding.Rotate]).Sens = -1;
        _stateMachine.ChangeState(_stateMachine.States[EnumHolding.Rotate]);
    }

    private void OnPush()
    {
        Sens = 1;
        _character.Animator.SetFloat("HoldingSens", Sens);
    }

    private void OnPull()
    {
        Sens = -1;
        _character.Animator.SetFloat("HoldingSens", Sens);
    }

}
