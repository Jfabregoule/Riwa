using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EmitterLaser : MonoBehaviour
{
    [SerializeField] private GameObject _particules;
    [SerializeField] private GameObject _impact;

    private LineRenderer _laser;
    private Vector3 _startPosition;
    private List<Vector3> _directions;
    private bool _isReflecting;
    void Start()
    {
        _laser = GetComponent<LineRenderer>();

        _startPosition = transform.position;
        _directions = new List<Vector3>();

        _isReflecting = true;

        ResetLaser();
    }

    void Update()
    {
        for (int i = 0; i < _laser.positionCount; i++)
        {
            if (i == _laser.positionCount - 1 && !_isReflecting)
            {
                continue;
            }

            if (Physics.Raycast(_laser.GetPosition(i), _directions[i], out RaycastHit hit, 10f))
            {
                if (IsPositionAlreadyAdded(hit.point)) continue;

                Vector3 reflect = Vector3.Reflect(_directions[i], hit.normal);

                AddLaserPoint(hit.point);
                _directions.Add(reflect);

                if (!hit.collider.GetComponent<RecepterLaser>())
                {
                    SpawnImpact(hit.point, hit.normal);
                    _isReflecting = false;
                }
                else
                {
                    _isReflecting = true;
                }
            }
            else if(i == _laser.positionCount - 1)
            {
                break;
            }
            else
            {
                RemoveLaserPoint(_laser.positionCount - i);
                break;
            }
        }
    }

    private void AddLaserPoint(Vector3 newPosition)
    {
        _laser.positionCount++;
        _laser.SetPosition(_laser.positionCount - 1, newPosition);
    }

    private void RemoveLaserPoint(int nbPoint)
    {
        _laser.positionCount -= nbPoint;
        _directions.RemoveRange(_directions.Count - nbPoint, nbPoint);

        if (_laser.positionCount == 0)
        {
            ResetLaser();
            return;
        }

        if (!_isReflecting)
        {
            _isReflecting = true;
            ResetImpact();
            return;
        }
    }


    private void ResetLaser()
    {
        _laser.positionCount = 0;
        AddLaserPoint(_startPosition);

        _directions.Clear();
        _directions.Add(transform.right);

        ResetImpact();
    }

    private void ResetImpact()
    {
        _impact.SetActive(false);
        _particules.SetActive(false);
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

    private void SpawnImpact(Vector3 position, Vector3 normal)
    {
        _impact.SetActive(true);
        _impact.transform.position = position + normal * 0.01f;
        _impact.transform.rotation = Quaternion.LookRotation(-normal);

        _particules.SetActive(true);
        _particules.transform.position = position;
        _particules.transform.rotation = Quaternion.LookRotation(normal);
    }

}
