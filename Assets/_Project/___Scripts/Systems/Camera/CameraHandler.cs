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

    [SerializeField] private Vector3 _cameraPos;
    [SerializeField] private float _offsetCameraMovement = 1f;
    [SerializeField] private float _speedCameraOffset;
    [SerializeField] private AnimationCurve _movementCameraCurve;

    private CinemachineFreeLook _freelookCamera;
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineConfiner _confinerCamera;

    private CinemachineTransposer _transposer;

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

    private void Start()
    {
        _freelookCamera = GetComponent<CinemachineFreeLook>();
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _confinerCamera = GetComponent<CinemachineConfiner>();
        _transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _cameraPos = new Vector3(0, 14, -11);

        _character = GameObject.Find("Character").GetComponent<ACharacter>();
        //_character = GameManager.Instance.Character; // IL FAUT UNE BONNE IMPLEMENTATION DU SYSTEM

        _lastJoystick = new Vector3(0,0,0);
        _startPosition = new Vector3(0,0,0);
        _targetPosition = new Vector3(0,0,0);

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
            }

            //Va définir le collider dans lequel la camera va se déplacer
            _confinerCamera.enabled = true;
            _confinerCamera.m_BoundingVolume = _setups[0]._colliderContain;

            _currentForward = _setups[0]._Xvalue;
            //_freelookCamera.m_XAxis.Value = _setups[0]._Xvalue;

        }

        //_transposer.m_FollowOffset = _cameraPos;
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
        _targetForward = _setups[id]._Xvalue;

        //_virtualCamera.enabled = false;
        //_freelookCamera.enabled = true;

        _clockRotate = 0;
        //StartCoroutine(CameraRotation());
    }

    public IEnumerator CameraRotation()
    {
        while (_clockRotate < 1)
        {
            _clockRotate += Time.deltaTime;

            float angle = Mathf.Lerp(_startForward, _targetForward, _clockRotate);
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
        //_transposer.m_FollowOffset = _cameraPos + _currentPosition;
        _character.CameraTarget.transform.localPosition = _cameraPos + _currentPosition;
    }

}
