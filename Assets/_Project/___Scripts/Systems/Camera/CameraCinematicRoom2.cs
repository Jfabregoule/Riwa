using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCinematicRoom2 : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;

    private AnimationCurve _yawByPathPosition;
    private CinemachineTrackedDolly _trackedDolly;
    private Vector3 _startRotation;
    private float _previousT = -1f;
    private int _direction = 1;
    private bool _isTraveling = false;


    void Awake()
    {
        _trackedDolly = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>();

        _startRotation = transform.rotation.eulerAngles;

        _yawByPathPosition = new AnimationCurve(
            new Keyframe(0f, -90f),
            new Keyframe(0.5f, -90f),
            new Keyframe(1f, 0f)
        );
    }

    void Update()
    {
        if (!_isTraveling || _trackedDolly == null) return;

        _trackedDolly.m_PathPosition += _direction * _speed * Time.deltaTime;

        _trackedDolly.m_PathPosition = Mathf.Clamp(_trackedDolly.m_PathPosition, 0f, _trackedDolly.m_Path.MaxPos);
    }

    void LateUpdate()
    {
        if (!_isTraveling || _trackedDolly == null) return;

        float t = _trackedDolly.m_PathPosition / _trackedDolly.m_Path.MaxPos;

        if (!Mathf.Approximately(t, _previousT))
        {
            float yaw = _yawByPathPosition.Evaluate(t);
            transform.rotation = Quaternion.Euler(_startRotation.x, yaw, _startRotation.z);
            _previousT = t;
        }
    }

    public void ReverseDirection()
    {
        _direction *= -1;
    }

    public IEnumerator StartTravel(bool forward)
    {
        _isTraveling = true;

        if (forward)
        {
            _direction = 1;
            yield return new WaitUntil(() => _trackedDolly.m_PathPosition >= _trackedDolly.m_Path.MaxPos);
        }
        else
        {
            _direction = -1;
            yield return new WaitUntil(() => _trackedDolly.m_PathPosition <= _trackedDolly.m_Path.MinPos);
        }
        _isTraveling = false;
    }
}
