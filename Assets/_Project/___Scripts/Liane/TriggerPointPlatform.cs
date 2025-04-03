using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TriggerPointPlatform : MonoBehaviour
{
    PlatformLiana _platform;
    private void Start()
    {
        _platform = transform.parent.GetComponent<PlatformLiana>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!other.TryGetComponent(out VineScript vineScript)) return;

        CapsuleCollider capsule = other.GetComponent<CapsuleCollider>();

        Vector3 WorldScale = capsule.transform.lossyScale;

        Vector3 CollisionPoint = other.ClosestPoint(transform.position);
        
        float WorldRadius = capsule.radius * WorldScale.y;

        Vector3 position = capsule.transform.TransformPoint(capsule.center);
        position.x = CollisionPoint.x;
        position.y += WorldRadius;
        vineScript.SetSocketChild(transform.parent);
        vineScript.SetSocketTransform(position);
    }
}
