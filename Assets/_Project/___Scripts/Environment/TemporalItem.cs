using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalItem : MonoBehaviour
{
    [SerializeField] private GameObject pastItem;
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private float positionThreshold = 0.1f;
    private float rotationThreshold = 0.1f;

    private void Start()
    {
        GameManager.Instance.OnTimeChangeStarted += ChangeCheck;
    }

    private void OnDestroy()
    {
        if(GameManager.Instance != null)
            GameManager.Instance.OnTimeChangeStarted -= ChangeCheck;
    }

    private void OnEnable()
    {
        if (HasSignificantChange())
        {
            transform.position = pastItem.transform.position;
            transform.rotation = pastItem.transform.rotation;
        }
    }

    private void ChangeCheck(EnumTemporality temporality)
    {
        if (temporality == EnumTemporality.Present)
        {   
            if (HasSignificantChange())
            {
                transform.position = pastItem.transform.position;
                transform.rotation = pastItem.transform.rotation;
            }
        }
        else
        {
            lastPosition = pastItem.transform.position;
            lastRotation = pastItem.transform.rotation;
        }
    }


    private bool HasSignificantChange()
    {
        if (Vector3.Distance(lastPosition, pastItem.transform.position) > positionThreshold)
            return true;

        if (Quaternion.Angle(lastRotation, pastItem.transform.rotation) > rotationThreshold)
            return true;

        return false;
    }
}
