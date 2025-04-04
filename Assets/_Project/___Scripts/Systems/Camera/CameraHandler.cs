using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public struct CameraSetup
{
    public Collider _colliderContain;
    public TriggerCamera _triggerCamera;
    public float _Xvalue;
}

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private List<CameraSetup> _setups;

    [Header("Stats")]

    [SerializeField] private float _offset = 1f;
    [SerializeField] private Vector3 _cameraPos;

    private CinemachineFreeLook _freelookCamera;
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineConfiner _confinerCamera;

    private CinemachineTransposer _transposer;

    private float _currentForward;
    private float _startForward;
    private float _targetForward;

    private float _clock;
    private Vector3 _currentPosition;
    private Vector3 _targetPosition;

    private void Start()
    {
        _freelookCamera = GetComponent<CinemachineFreeLook>();
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _confinerCamera = GetComponent<CinemachineConfiner>();
        _transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _cameraPos = new Vector3(0, 16, -13);

        if (_setups.Count == 0)
        {
            _confinerCamera.enabled = false;
            return;
        }

        for(int i = 0; i < _setups.Count; i++)
        {
            _setups[i]._triggerCamera.OnExitTrigger += RotateCam;
        }

        //Va définir le collider dans lequel la camera va se déplacer
        _confinerCamera.enabled = true;
        _confinerCamera.m_BoundingVolume = _setups[0]._colliderContain;

        _currentForward = _setups[0]._Xvalue;
        _freelookCamera.m_XAxis.Value = _setups[0]._Xvalue;

        _transposer.m_FollowOffset = _cameraPos;
        _currentPosition = _cameraPos;
    }

    public void Update()
    {
        if (Mathf.Approximately(_currentPosition.x,_targetPosition.x) && Mathf.Approximately(_currentPosition.y, _targetPosition.y) && Mathf.Approximately(_currentPosition.z, _targetPosition.z)) {

            Vector3 direction = GameManager.Instance.Joystick.Direction;

            _targetPosition = _cameraPos + direction * _offset;

            _currentPosition = Vector3.Lerp(_currentPosition, _targetPosition, Time.deltaTime);

            _transposer.m_FollowOffset = _currentPosition;  

        }
    }

    public void RotateCam(int id)
    {
        _confinerCamera.m_BoundingVolume = _setups[id]._colliderContain;

        _startForward = _currentForward;
        _targetForward = _setups[id]._Xvalue;

        //_virtualCamera.enabled = false;
        //_freelookCamera.enabled = true;

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

        //_freelookCamera.enabled = false;
        //_virtualCamera.enabled = true;

    }

}
