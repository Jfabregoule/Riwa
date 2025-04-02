using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EmitterLaser : MonoBehaviour
{
    [SerializeField] private Material _materialLaser;
    private LineRenderer _laser;
    private Vector3 _startPosition;
    private int _layerMask;
    void Start()
    {
        _laser = transform.AddComponent<LineRenderer>();
        _laser.material = _materialLaser;
        _startPosition = transform.position;
        _layerMask = ~LayerMask.GetMask("Ignore Raycast");

        ResetLaser();
    }

    void Update()
    {
        if (Physics.Raycast(_startPosition, transform.right, out RaycastHit hit, 10f, _layerMask))
        {
            if (IsPositionAlreadyAdded(hit.point)) return;

            ResetLaser();
            AddLaserPoint(hit.point);
        }
        else if(_laser.positionCount > 0)
        {
            ResetLaser();
        }
    }

    private void AddLaserPoint(Vector3 newPosition)
    {
        _laser.positionCount++;
        _laser.SetPosition(_laser.positionCount - 1, newPosition);
    }


    private void ResetLaser()
    {
        _laser.positionCount = 0;
        AddLaserPoint(_startPosition);
    }

    bool IsPositionAlreadyAdded(Vector3 position)
    {
        for (int i = 0; i < _laser.positionCount; i++)
        {
            if (_laser.GetPosition(i) == position)
            {
                return true;
            }
        }
        return false;
    }

}
