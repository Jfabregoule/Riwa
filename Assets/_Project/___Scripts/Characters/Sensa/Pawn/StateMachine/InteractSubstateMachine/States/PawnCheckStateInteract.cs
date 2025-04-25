using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PawnCheckStateInteract<TStateEnum> : PawnInteractBaseSubstate<TStateEnum>
    where TStateEnum : Enum
{

    protected List<GameObject> _colliderList = new();
    protected bool _canInteract = false;

    protected List<Type> _possibleTypes;

    public override void InitState(PawnInteractSubstateMachine<TStateEnum> stateMachine, EnumInteract enumValue, APawn<TStateEnum> character)
    {
        base.InitState(stateMachine, enumValue, character);
    }

    public override void EnterState()
    {
        base.EnterState();

        _colliderList.Clear();

        float security = _character.InteractRadius;

        CapsuleCollider collider2 = GameManager.Instance.Character.CapsuleCollider;

        float height = _character.CapsuleCollider.height * _character.transform.localScale.x;
        float radius = _character.CapsuleCollider.radius * _character.transform.localScale.x * security;

        Vector3 point1 = _character.transform.position;
        Vector3 point2 = _character.transform.position + Vector3.up * height;

        // Offset the capsule forward to scan ahead of the player
        Vector3 forwardOffset = _character.transform.forward * radius * 0.2f;
        point1 += forwardOffset;
        point2 += forwardOffset;

        LayerMask layerMask = GameManager.Instance.CurrentTemporality == EnumTemporality.Past ? _character.PastLayer : _character.PresentLayer;

        Collider[] others = Physics.OverlapCapsule(point1, point2, radius, layerMask);

        // Debug Draws to visualize the capsule and area
        Debug.DrawLine(point2, point2 + Vector3.right * radius, Color.white, 10f);
        Debug.DrawLine(point1, point1 + Vector3.right * radius, Color.magenta, 10f);

        Debug.DrawLine(point2, point2 + Vector3.forward * radius, Color.white, 10f);
        Debug.DrawLine(point1, point1 + Vector3.forward * radius, Color.magenta, 10f);

        Debug.DrawLine(point2, point2 + -Vector3.right * radius, Color.white, 10f);
        Debug.DrawLine(point1, point1 + -Vector3.right * radius, Color.magenta, 10f);

        Debug.DrawLine(point2, point2 + -Vector3.forward * radius, Color.white, 10f);
        Debug.DrawLine(point1, point1 + -Vector3.forward * radius, Color.magenta, 10f);

        Vector3 sphereCastOrigin = _character.transform.position + Vector3.up * (height * 0.5f);
        float sphereCastRadius = 0.2f; // Thickness of the spherecast

        foreach (Collider collider in others)
        {
            if (collider.gameObject.TryGetComponent(out IInteractableBase obj))
            {
                if (!obj.CanInteract) continue;

                Vector3 targetPoint = collider.bounds.center;
                Vector3 direction = (targetPoint - sphereCastOrigin).normalized;
                float distance = Vector3.Distance(sphereCastOrigin, targetPoint) + 0.5f;

                if (Physics.SphereCast(sphereCastOrigin, sphereCastRadius, direction, out RaycastHit hit, distance, layerMask))
                {
                    if (hit.collider.gameObject == collider.gameObject)
                    {
                        _colliderList.Add(collider.gameObject);
                    }
                }
            }
        }

        if (_colliderList.Count <= 0)
        {
            // No interactable objects found
            _canInteract = false;
            return;
        }

        //A FIX
        _colliderList = SortPriority(_colliderList);

        _subStateMachine.CurrentObjectInteract = SortObjects(_character.transform.position, _colliderList);
        _canInteract = true;
    }

    public override void ExitState()
    {
        base.ExitState();
        _canInteract = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void CheckChangeState()
    {
        base.CheckChangeState();
        
        if (_canInteract)
        {
            ChangeStateToMove();
            _character?.InteractBegin();
        }
        else
            ChangeStateToIdle();
    }

    public virtual void ChangeStateToIdle() { }
    public virtual void ChangeStateToMove() { }

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

    public List<GameObject> SortPriority(List<GameObject> objs)
    {
        List<GameObject> result = new();
        int highestPrio = 0;

        for (int i = 0; i < objs.Count; i++)
        {
            if (IsValidType(objs[i].GetComponent<IInteractableBase>()))
            {
                if (objs[i].GetComponent<IInteractableBase>().Priority > highestPrio)
                {
                    highestPrio = objs[i].GetComponent<IInteractableBase>().Priority;
                }
            }
        }

        for (int i = 0; i < objs.Count; i++)
        {
            if (IsValidType(objs[i].GetComponent<IInteractableBase>()))
            {
                if (objs[i].GetComponent<IInteractableBase>().Priority == highestPrio)
                {
                    GameObject go = objs[i].gameObject;
                    result.Add(go);
                }
            }
        }

        return result;

    }

    public virtual bool IsValidType(IInteractableBase interact)
    {
        Type objType = interact.GetType();

        foreach (var interfaceType in _possibleTypes)
        {
            if(ImplementsExactly(objType, interfaceType))
            {
                return true;
            }
        }
        return false;
    }

    bool ImplementsExactly(Type concreteType, Type interfaceType)
    {
        var allInterfaces = concreteType.GetInterfaces();
        var baseInterfaces = concreteType.BaseType != null ? concreteType.BaseType.GetInterfaces() : Array.Empty<Type>();

        var directInterfaces = allInterfaces.Except(baseInterfaces);

        return directInterfaces.Contains(interfaceType);
    }

}
