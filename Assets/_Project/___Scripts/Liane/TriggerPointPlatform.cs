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
        if (capsule == null) return; // Sécurité si jamais le collider n'est pas trouvé

        Vector3 WorldScale = transform.lossyScale;

        Vector3 CollisionPoint = other.ClosestPoint(transform.position);
        
        float WorldRadius = capsule.radius * WorldScale.y;

        //float worldHeightY = capsule.height;
        //Vector3 pointPosition = new Vector3(CollisionPoint.x, WorldRadius, CollisionPoint.z);
        Vector3 position = capsule.transform.TransformPoint(capsule.center);
        vineScript.SetSocketTransform(capsule.transform.TransformPoint(capsule.center));

        //Instantiate(null, new Vector3());
        //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //sphere.transform.position = pointPosition;
    }
}
