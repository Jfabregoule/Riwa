using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private Collider _colliderContain1;
    [SerializeField] private Collider _colliderContain2;

    [SerializeField] private TriggerCamera _triggerCamera1;
    [SerializeField] private TriggerCamera _triggerCamera2;

    [SerializeField] private float _startXValue;
    [SerializeField] private float _secondXValue;

    private CinemachineFreeLook _freelookCamera;
    private CinemachineConfiner _confinerCamera;

    private float _currentForward;

    private void Start()
    {
        _freelookCamera = GetComponent<CinemachineFreeLook>();
        _confinerCamera = GetComponent<CinemachineConfiner>();

        _currentForward = _startXValue;
        _freelookCamera.m_XAxis.Value = _startXValue;

        _triggerCamera1.OnExitTrigger += RotateCam;
        _triggerCamera2.OnExitTrigger += RotateCam;

    }

    public void RotateCam()
    {
        if (_freelookCamera.m_XAxis.Value == _startXValue)
        {
            _freelookCamera.m_XAxis.Value = _secondXValue;
            _confinerCamera.m_BoundingVolume = _colliderContain2;
        }
        else
        {
            _freelookCamera.m_XAxis.Value = _startXValue;
            _confinerCamera.m_BoundingVolume = _colliderContain1;
        }

    }




}
