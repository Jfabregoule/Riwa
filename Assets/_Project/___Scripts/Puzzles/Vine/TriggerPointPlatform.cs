using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TriggerPointPlatform : MonoBehaviour
{
    private List<VineScript> overlappingVines = new();

    private void OnTriggerEnter(Collider other)
    {

        if (!other.TryGetComponent(out VineScript vineScript)) return;

        if (!overlappingVines.Contains(vineScript))
            overlappingVines.Add(vineScript);

        CapsuleCollider capsule = other.GetComponent<CapsuleCollider>();

        Vector3 WorldScale = capsule.transform.lossyScale;

        Vector3 CollisionPoint = other.ClosestPoint(transform.position);


        float WorldRadius = capsule.radius * WorldScale.y;

        Vector3 position = capsule.transform.TransformPoint(capsule.center);
        position.x = CollisionPoint.x;
        position.y += WorldRadius;
        position.z = CollisionPoint.z;

        transform.position = position;
        vineScript.SetSocketTransform(transform);
        VineManager.Instance.InvokeVineChange();
        VineManager.Instance.OnVineChange += vineScript.SetSocketPoint;
        Debug.Log(overlappingVines.Count);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out VineScript vineScript)) return;

        overlappingVines.Remove(vineScript);

        if (overlappingVines.Count == 0)
        {
            vineScript.SetSocketNull();
        }
    }


}
