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
    public float _angle;
}

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private List<CameraSetup> _setups;

    [Header("Stats")]

    private Vector3 _cameraPos;
    private Vector3 _cameraRotation;

    [SerializeField] private float _offsetCameraMovement = 1f;
    [SerializeField] private float _speedCameraOffset;
    [SerializeField] private float _rotationSpeed;

    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineConfiner _confinerCamera;

    private ACharacter _character;

    private float _currentForward;
    private float _startForward;
    private float _targetForward;
    private float _clockRotate;

    private float _clockPosition;   
    private Vector3 _startPosition;
    private Vector3 _currentPosition;
    private Vector3 _targetPosition;
    private Vector3 _lastJoystick;

    private float _radius;

    private void Start()
    {
        _character = GameObject.Find("Character").GetComponent<ACharacter>();
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _confinerCamera = GetComponent<CinemachineConfiner>();
        _cameraPos = _character.CameraTarget.transform.localPosition;
        _cameraRotation = transform.localEulerAngles;

        //_character = GameManager.Instance.Character; // IL FAUT UNE BONNE IMPLEMENTATION DU SYSTEM

        _lastJoystick = new Vector3(0,0,0);
        _startPosition = new Vector3(0,0,0);
        _targetPosition = new Vector3(0,0,0);

        _radius = _cameraPos.z;

        if (_setups.Count == 0)
        {
            _confinerCamera.enabled = true;
            return;
        }
        else
        {
            for (int i = 0; i < _setups.Count; i++)
            {
                _setups[i]._triggerCamera.OnExitTrigger += RotateCam;
                _setups[i]._triggerCamera.Id = i;
            }

            //Va définir le collider dans lequel la camera va se déplacer
            _confinerCamera.enabled = true;
            _confinerCamera.m_BoundingVolume = _setups[0]._colliderContain;

            _currentForward = _setups[0]._angle;

        }

        _currentPosition = _cameraPos;
    }

    public void Update()
    {
        if (_character.FsmCharacter.CurrentState.EnumState == EnumStateCharacter.Move ||
           _character.FsmCharacter.CurrentState.EnumState == EnumStateCharacter.SoulWalk)
        {
            MoveCameraOffset();
        }
    }


    public void RotateCam(int id)
    {
        _confinerCamera.m_BoundingVolume = _setups[id]._colliderContain;

        _startForward = _currentForward;
        _targetForward = _setups[id]._angle;

        _clockRotate = 0;
        StartCoroutine(CameraRotation());
    }

    public IEnumerator CameraRotation()
    {
        while (_clockRotate < 1)
        {
            _clockRotate += Time.deltaTime;

            float angle = Mathf.Lerp(_startForward, _targetForward, _clockRotate);

            float radians = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Sin(radians) * _radius, _cameraPos.y, Mathf.Cos(radians) * _radius);

            //_character.CameraTarget.transform.localPosition = offset;
            //_cameraPos = offset;

            transform.localEulerAngles = new Vector3(0, angle, 0) + _cameraRotation;

            _character.CameraTargetParent.transform.localEulerAngles = new Vector3(0,angle,0);
            _currentForward = angle;
            
            yield return null;
        }

    }

    public void MoveCameraOffset()
    {
        //Jsuis content jlai fait sans chat gpt (ptet pas opti de faire lerp dans update mais vu qu'il se fait quasi tout le temps à voir)
        //C'est pour avoir un leger offset smooth vers la ou on se dirige

        if (GameManager.Instance.Character.Rb.velocity != _lastJoystick/* && GameManager.Instance.Character.Rb.velocity != Vector3.zero*/)
        {
            _clockPosition = 0;

            _lastJoystick = new Vector3(GameManager.Instance.Character.Rb.velocity.x, 0, GameManager.Instance.Character.Rb.velocity.z);


            _targetPosition = _lastJoystick * _offsetCameraMovement;
            _startPosition = _currentPosition;

        }

        //Update clock
        _clockPosition += Time.deltaTime * _speedCameraOffset;
        _clockPosition = Mathf.Clamp(_clockPosition, 0, 1);

        //_currentPosition = Vector3.Lerp(_startPosition, _targetPosition, _movementCameraCurve.Evaluate(_clockPosition));
        _currentPosition = Vector3.Lerp(_startPosition, _targetPosition, _clockPosition);

        //On set l'offset ici 
        _character.CameraTarget.transform.localPosition = _cameraPos + _currentPosition;
    }

}
