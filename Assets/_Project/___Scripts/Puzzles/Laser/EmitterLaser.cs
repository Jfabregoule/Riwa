using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterLaser : MonoBehaviour
{
    [SerializeField] private GameObject _particules;
    [SerializeField] private GameObject _impact;
    [SerializeField] private Transform _startPoint;

    private LineRenderer _laser;
    private List<Vector3> _directions;
    private bool _isReflecting;
    private bool _isActive;
    private int _layerMask;

    [SerializeField] private MonoBehaviour[] _activables;
    private int CurrentActive;

    void Start()
    {
        _laser = GetComponent<LineRenderer>();

        _directions = new List<Vector3>();

        int layerToExclude = LayerMask.NameToLayer("whatIsPresentObject");
        _layerMask = ~(1 << layerToExclude);

        foreach (var activable in _activables)
        {
            if (activable.TryGetComponent(out IActivable act))
            {
                act.OnActivated += AddActivate;
                act.OnDesactivated += RemoveActivate;
            }
        }

        ResetLaser();
    }

    void Update()
    {
        if (!_isActive) return;

        ResetLaser();

        for (int i = 0; i < _laser.positionCount; i++)
        {
            if (!_isReflecting) continue;

            if (Physics.Raycast(_laser.GetPosition(i), _directions[i], out RaycastHit hit, 100f, _layerMask))
            {    
                Vector3 reflect = Vector3.Reflect(_directions[i], hit.normal);

                AddLaserPoint(hit.point);
                _directions.Add(reflect);

                if (!hit.collider.TryGetComponent(out Mirror mirror) || hit.normal != -mirror.transform.right)
                {
                    SpawnImpact(hit.point, hit.normal);
                    _isReflecting = false;

                    if (hit.collider.TryGetComponent(out RecepterLaser recepter))
                    {
                        recepter.OnLaserHit();
                    }
                }
            }
            else if (i == _laser.positionCount - 1)
            {
                break;
            }
            else
            {
                RemoveLaserPoint(_laser.positionCount - i - 1);
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
        AddLaserPoint(_startPoint.position);

        _directions.Clear();
        _directions.Add(_startPoint.forward);

        _isReflecting = true;
        ResetImpact();
    }

    private void ResetImpact()
    {
        _impact.SetActive(false);
        _particules.SetActive(false);
    }

    private void SpawnImpact(Vector3 position, Vector3 normal)
    {
        _impact.SetActive(true);
        _particules.SetActive(true);
        
        _impact.transform.position = position + normal * 0.01f;
        _impact.transform.rotation = Quaternion.LookRotation(-normal);

        _particules.transform.position = position;
        _particules.transform.rotation = Quaternion.LookRotation(normal);
    }

    private void AddActivate()
    {
        CurrentActive++;
        if (CurrentActive == _activables.Length)
        {
            _isActive = true;
        }
    }

    private void RemoveActivate()
    {
        if (CurrentActive == _activables.Length)
        {
            _isActive = false;
            ResetLaser();
        }
        CurrentActive--;
    }
}
