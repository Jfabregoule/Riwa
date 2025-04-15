using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

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

    private Coroutine _currentCoroutine;
    private float _clockZoom;

    private GameObject _cameraTargetParent;
    private GameObject _cameraTarget;

    public CinemachineVirtualCamera VirtualCamera { get => _virtualCamera;}

    private void Awake()
    {
        _cameraTargetParent = transform.parent.transform.parent.gameObject;
        _cameraTarget = transform.parent.gameObject;

    }

    private void Start()
    {
        _character = GameManager.Instance.Character;
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _confinerCamera = GetComponent<CinemachineConfiner>();
        _cameraPos = _cameraTarget.transform.localPosition;
        _cameraRotation = transform.localEulerAngles;

        _lastJoystick = new Vector3(0,0,0);
        _startPosition = new Vector3(0,0,0);
        _targetPosition = new Vector3(0,0,0);

        _radius = _cameraPos.z;

        if (_confinerCamera.m_BoundingVolume == null)
        {
            _confinerCamera.enabled = false;
        }
        else if (_setups.Count == 0)
        {
            _confinerCamera.enabled = true;
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
        if (_character.StateMachine.CurrentState.EnumState == EnumStateCharacter.Move) 
        {
            MoveCameraOffset();
        }
    }

    public void LateUpdate()
    {
        _cameraTargetParent.transform.position = _character.transform.position;
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

            //transform.localEulerAngles = new Vector3(0, angle, 0) + _cameraRotation;

            _cameraTargetParent.transform.localEulerAngles = new Vector3(0,angle,0);
            _currentForward = angle;
            
            yield return null;
        }

    }

    public void MoveCameraOffset()
    {
        //Jsuis content jlai fait sans chat gpt (ptet pas opti de faire lerp dans update mais vu qu'il se fait quasi tout le temps à voir)
        //C'est pour avoir un leger offset smooth vers la ou on se dirige
        //Update: j'ai utilisé le chat pour localCamVelocity

        Vector3 playerMovement = GameManager.Instance.Character.Rb.velocity;

        if (playerMovement != _lastJoystick/* && GameManager.Instance.Character.Rb.velocity != Vector3.zero*/)  
        {
            _clockPosition = 0;

            Vector3 movement = GameManager.Instance.Character.Rb.velocity;
            Vector3 camForward = transform.forward;
            Vector3 camRight = transform.right;

            camForward.y = 0;
            camRight.y = 0;

            camForward.Normalize();
            camRight.Normalize();

            float x = Vector3.Dot(playerMovement, camRight);
            float z = Vector3.Dot(playerMovement, camForward);
            Vector3 localCamVelocity = new Vector3(x, 0, z);

            _lastJoystick = localCamVelocity;

            _targetPosition = _lastJoystick * _offsetCameraMovement;
            _startPosition = _currentPosition;

        }

        //Update clock
        _clockPosition += Time.deltaTime * _speedCameraOffset;
        _clockPosition = Mathf.Clamp(_clockPosition, 0, 1);

        //_currentPosition = Vector3.Lerp(_startPosition, _targetPosition, _movementCameraCurve.Evaluate(_clockPosition));
        _currentPosition = Vector3.Lerp(_startPosition, _targetPosition, _clockPosition);

        //On set l'offset ici 
        _cameraTarget.transform.localPosition = _cameraPos + _currentPosition;
    }

    public void OnZoomCamera(float startZoom, float EndZoom)
    {
        if(_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        _clockZoom = 0;
        _currentCoroutine = StartCoroutine(ZoomCamera(startZoom, EndZoom));
        
    }

    public IEnumerator ZoomCamera(float startZoom, float EndZoom)
    {
        while (_clockZoom < 1)
        {
            _clockZoom += Time.deltaTime * 0.5f;

            GameManager.Instance.CameraHandler.VirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(startZoom, EndZoom, _clockZoom);

            yield return null;

        }
    }

    public void ResetLookAt()
    {
        _virtualCamera.LookAt = null;
        transform.localEulerAngles = _cameraRotation;
    }

}
