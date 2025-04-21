using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField] private CameraHandler _cameraHandler;
    [SerializeField] private ACharacter _character;
    [SerializeField] private VariableJoystick _joystick;

    [Header("Player setup")]

    [SerializeField] private Vector3 _playerSpawnPosition;
    [SerializeField] private Vector3 _playerSpawnRotation;

    public void OnEnable()
    {
        GameManager.Instance.Load3C(_cameraHandler, _character, _joystick);

        _character.RespawnPosition = _playerSpawnPosition;
        _character.RespawnRotation = _playerSpawnRotation;
    }

    public void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(gameObject.scene.name));
    }
}
