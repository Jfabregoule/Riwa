using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TriggerPointPlatform : MonoBehaviour
{
    private readonly List<VineScript> _triggerVines = new List<VineScript>();
    private VineScript _currentVine;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out VineScript vineScript)) return;

        if (!_triggerVines.Contains(vineScript))
            _triggerVines.Add(vineScript);

        if (_currentVine != vineScript)
        {
            if (_currentVine != null)
                _currentVine.SetSocketNull();

            _currentVine = vineScript;
            SetPosition(vineScript);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out VineScript vineScript)) return;

        _triggerVines.Remove(vineScript);

        if (_currentVine == vineScript)
        {
            _currentVine.SetSocketNull();
            _currentVine = null;

            gameObject.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionY;
            gameObject.GetComponent<Rigidbody>().useGravity = true;

            if (_triggerVines.Count > 0)
            {
                _currentVine = _triggerVines[_triggerVines.Count - 1];
                SetPosition(_currentVine);
                gameObject.GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezePositionY;
                gameObject.GetComponent<Rigidbody>().useGravity = false;
            }
        }
    }

    private void SetPosition(VineScript vine)
    {
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
