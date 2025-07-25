using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EnumCameraRoom
{
    Main,
    LookAtTree,
    LookAtDoor,
    LastCameraRoom1,
}

[System.Serializable]
public struct RoomCamera
{
    public CinemachineVirtualCamera _camera;
    public EnumCameraRoom _id;
}

public class BaseLevelManager : Singleton<BaseLevelManager>
{
    /// <summary>
    /// Chaque level aura un enfant de ce script
    /// Ce Manager va concerner l'avancée de la scene à un niveau logique
    /// 
    /// Contient les 3C du niveau (mettre en enfant du gameObject du manager les 3C pour y accéder plus facilement ?)
    /// 
    /// </summary>

    public CinemachineBrain CinemachineBrainCamera;

    [Header("Les 3C")]

    [SerializeField] protected CameraHandler _cameraHandler;
    [SerializeField] protected ACharacter _character;
    [SerializeField] protected VariableJoystick _joystick;

    [Header("Player setup")]

    [SerializeField] protected Vector3 _playerSpawnPosition;
    [SerializeField] protected Vector3 _playerSpawnRotation;

    [Header("Camera")]

    [SerializeField] protected List<RoomCamera> _cameras;
    public readonly Dictionary<EnumCameraRoom, CinemachineVirtualCamera> CameraDictionnary = new();

    public delegate void LevelEvent();
    public event LevelEvent OnLevelEnter;

    public virtual void OnEnable()
    {
        GameManager.Instance.Load3C(_cameraHandler, _character, _joystick);

        //_character.RespawnPosition = _playerSpawnPosition;
        //_character.RespawnRotation = _playerSpawnRotation;

        GameManager.Instance.CurrentLevelManager = this;
    }

    public virtual void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(gameObject.scene.name));

        if (GameManager.Instance.CurrentTemporality == EnumTemporality.Present)
        {
            Helpers.Camera.cullingMask |= 1 << 7;
            Physics.IgnoreLayerCollision(_character.gameObject.layer, Mathf.Clamp(Mathf.RoundToInt(Mathf.Log(_character.PastLayer.value, 2)), 0, 31), true);
            Physics.IgnoreLayerCollision(_character.gameObject.layer, Mathf.Clamp(Mathf.RoundToInt(Mathf.Log(_character.PresentLayer.value, 2)), 0, 31), false);
        }
        else
        {
            Helpers.Camera.cullingMask |= 1 << 6;
            Physics.IgnoreLayerCollision(_character.gameObject.layer, Mathf.Clamp(Mathf.RoundToInt(Mathf.Log(_character.PresentLayer.value, 2)), 0, 31), true);
            Physics.IgnoreLayerCollision(_character.gameObject.layer, Mathf.Clamp(Mathf.RoundToInt(Mathf.Log(_character.PastLayer.value, 2)), 0, 31), false);
        }
    }

    public virtual void LevelEnter()
    {
        OnLevelEnter?.Invoke();
    }
}
