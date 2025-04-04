using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
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
    private float _startForward;
    private float _targetForward;

    private float _clock;

    private void Start()
    {
        _freelookCamera = GetComponent<CinemachineFreeLook>();
        _confinerCamera = GetComponent<CinemachineConfiner>();

        _currentForward = _startXValue;
        _freelookCamera.m_XAxis.Value = _startXValue;

        _triggerCamera1.OnExitTrigger += RotateCam2;
        _triggerCamera2.OnExitTrigger += RotateCam1;

    }

    public void RotateCam1()
    {
        _confinerCamera.m_BoundingVolume = _colliderContain1;

        _startForward = _secondXValue;
        _targetForward = _startXValue;

        _clock = 0;
        StartCoroutine(CameraRotation());
    }

    public void RotateCam2()
    {
        _confinerCamera.m_BoundingVolume = _colliderContain2;

        _startForward = _startXValue;
        _targetForward = _secondXValue;

        _clock = 0;
        StartCoroutine(CameraRotation());

    }

    public IEnumerator CameraRotation()
    {
        while (_clock < 1)
        {
            _clock += Time.deltaTime;

            float angle = Mathf.Lerp(_startForward, _targetForward, _clock);
            _freelookCamera.m_XAxis.Value = angle;

            Vector3 vect = _freelookCamera.gameObject.transform.eulerAngles;
            vect.y = angle;
            _freelookCamera.gameObject.transform.eulerAngles = vect;

            _currentForward = angle;

            yield return null;
        }

    }

}
