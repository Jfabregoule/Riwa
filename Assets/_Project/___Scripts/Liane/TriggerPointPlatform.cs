using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        CapsuleCollider capsuleCollider = other.GetComponent<CapsuleCollider>();


        Debug.Log(other.ClosestPoint(transform.position));

        //Instantiate(null, new Vector3());
        //_platform.SetPlatformInfos(vineScript.GetSpline(), vineScript.FrictionSpeed);
    }
}
