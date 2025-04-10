using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TriggerPointPlatform : MonoBehaviour
{
    private readonly List<VineScript> _triggerVines = new List<VineScript>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out VineScript vineScript)) return;

        Debug.Log("ça rentres");

        //CapsuleCollider capsule = other.GetComponent<CapsuleCollider>();

        //Vector3 WorldScale = capsule.transform.lossyScale;

        //Vector3 CollisionPoint = other.ClosestPoint(transform.position);

        //float WorldRadius = capsule.radius * WorldScale.y;

        //Vector3 position = capsule.transform.TransformPoint(capsule.center);
        //position.x = CollisionPoint.x;
        //position.y += WorldRadius;
        //position.z = CollisionPoint.z;

        //transform.position = position;
        //vineScript.SetSocketTransform(transform);

        SetPosition(vineScript);

        if (_triggerVines.Count > 0)
        {
            _triggerVines[_triggerVines.Count - 1].SetSocketPoint();
            Debug.Log("reset ancienne");
        }
        _triggerVines.Add(vineScript);

        //if(_triggerVine != null)
        //{
        //    _triggerVine.SetSocketPoint();
        //}
        //_triggerVine = vineScript;

        //Debug.Log(VineManager.Instance.TriggerVines.Count);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out VineScript vineScript)) return;

        Debug.Log("ça sort, reset");

        vineScript.SetSocketPoint();
        _triggerVines.Remove(vineScript);

        if (_triggerVines.Count == 0)
        {
            gameObject.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionY;
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            Debug.Log("null");
        }
        else
        {
            SetPosition(_triggerVines[_triggerVines.Count - 1]);
        }
    }

    private void SetPosition(VineScript vine)
    {
        Debug.Log("set");
        CapsuleCollider capsule = vine.gameObject.GetComponent<CapsuleCollider>();

        Vector3 WorldScale = capsule.transform.lossyScale;

        Vector3 CollisionPoint = capsule.ClosestPoint(transform.position);

        float WorldRadius = capsule.radius * WorldScale.y;

        Vector3 position = capsule.transform.TransformPoint(capsule.center);
        position.x = CollisionPoint.x;
        position.y += WorldRadius;
        position.z = CollisionPoint.z;

        transform.position = position;
        vine.SetSocketTransform(transform);
    }
}
