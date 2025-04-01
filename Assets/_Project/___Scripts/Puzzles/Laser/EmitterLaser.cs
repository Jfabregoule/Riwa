using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EmitterLaser : MonoBehaviour
{
    private GameObject _cylinder;
    private Vector3 _startPosition;
    private float _lastDistance;
    private int _layerMask;
    void Start()
    {
        _cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        _cylinder.transform.SetParent(transform);
        _cylinder.transform.up = transform.right;
        _cylinder.transform.localScale = new Vector3(0.3f, 0f, 0.3f);
        _cylinder.layer = 2;

        _startPosition = transform.position;
        _layerMask = ~LayerMask.GetMask("Ignore Raycast");

        ResetLaserDistance();
    }

    void Update()
    {
        if (Physics.Raycast(_startPosition, transform.right, out RaycastHit hit, 10f, _layerMask))
        {
            float distance = Vector3.Distance(hit.point, _startPosition);
            if (!Mathf.Approximately(distance, _lastDistance))
            {
                UpdateLaserDistance(distance);
            }
        }
        else if (!Mathf.Approximately(0f, _lastDistance))
        {
            ResetLaserDistance();
        }
        //Debug.DrawRay(transform.position, transform.right * 10f, Color.red);
    }

    private void UpdateLaserDistance(float distance)
    {
        _lastDistance = distance;

        _cylinder.transform.localScale = new Vector3(
            _cylinder.transform.localScale.x, 
            (distance) * 0.5f, 
            _cylinder.transform.localScale.z
        );

        _cylinder.transform.position = _startPosition + transform.right * (distance * 0.5f);
    }

    private void ResetLaserDistance()
    {
        UpdateLaserDistance(0f);

        //_lastDistance = 0f;
        //_cylinder.transform.localScale = new Vector3(0.3f, 0f, 0.3f);
        //_cylinder.transform.position = _pointToStart.position;
    }
}
