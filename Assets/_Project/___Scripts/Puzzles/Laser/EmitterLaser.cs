using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class EmitterLaser : MonoBehaviour
{
    [SerializeField] private GameObject _particules;
    [SerializeField] private GameObject _impact;

    private LineRenderer _laser;
    private List<Vector3> _directions;
    private bool _isReflecting;
    private bool _isActive;

    [SerializeField] private MonoBehaviour[] _activables;
    private int CurrentActive;

    void Start()
    {
        _laser = GetComponent<LineRenderer>();

        _directions = new List<Vector3>();

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
        if (_isActive)
        {
            ResetLaser();
            for (int i = 0; i < _laser.positionCount; i++)
            {
                if (!_isReflecting) continue;

                if (Physics.Raycast(_laser.GetPosition(i), _directions[i], out RaycastHit hit, 10f))
                {
                    Vector3 reflect = Vector3.Reflect(_directions[i], hit.normal);

                    AddLaserPoint(hit.point);
                    _directions.Add(reflect);

                    if (!hit.collider.GetComponent<Mirror>())
                    {
                        SpawnImpact(hit.point, hit.normal);
                        _isReflecting = false;

                        if (hit.collider.TryGetComponent<RecepterLaser>(out var recepter))
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
        AddLaserPoint(transform.position);

        _directions.Clear();
        _directions.Add(transform.right);

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
        _impact.transform.position = position + normal * 0.01f;
        _impact.transform.rotation = Quaternion.LookRotation(-normal);

        _particules.SetActive(true);
        _particules.transform.position = position;
        _particules.transform.rotation = Quaternion.LookRotation(normal);
    }

    //private IEnumerator EmitteLaser()
    //{
    //    ResetLaser();
    //    for (int i = 0; i < _laser.positionCount; i++)
    //    {
    //        if (!_isReflecting) continue;

    //        if (Physics.Raycast(_laser.GetPosition(i), _directions[i], out RaycastHit hit, 10f))
    //        {
    //            Vector3 reflect = Vector3.Reflect(_directions[i], hit.normal);

    //            AddLaserPoint(hit.point);
    //            _directions.Add(reflect);

    //            if (!hit.collider.GetComponent<Mirror>())
    //            {
    //                SpawnImpact(hit.point, hit.normal);
    //                _isReflecting = false;

    //                if (hit.collider.TryGetComponent<RecepterLaser>(out var recepter))
    //                {
    //                    recepter.OnLaserHit();
    //                }
    //            }
    //        }
    //        else if (i == _laser.positionCount - 1)
    //        {
    //            break;
    //        }
    //        else
    //        {
    //            RemoveLaserPoint(_laser.positionCount - i - 1);
    //            break;
    //        }
    //    }
    //}

    //public void Activate()
    //{
    //    _isActive = true;
    //    //StartCoroutine(EmitteLaser());
    //}

    //public void Deactivate()
    //{
    //    _isActive = false;
    //    //StopCoroutine(EmitteLaser());
    //}

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
        }
        CurrentActive--;
    }
}
