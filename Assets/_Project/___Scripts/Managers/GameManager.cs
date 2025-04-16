using UnityEngine;

[DefaultExecutionOrder(-10)]
public class GameManager : Singleton<GameManager>
{
    private const string PLAYER_TAG = "Player";
    private const string SOUND_MANAGER_TAG = "SoundManager";
    private const string MAIN_CAMERA_TAG = "MainCamera";
    private const string TRANSLATE_TAG = "TranslateSystem";

    [HideInInspector] public GameObject MainCamera;
    [HideInInspector] public SoundSystem SoundSystem;
    [HideInInspector] public TranslateSystem TranslateSystem;

    private CameraHandler _cameraHandler;
    private ACharacter _character;
    private VariableJoystick _joystick;

    #region Properties

    public CameraHandler CameraHandler { get => _cameraHandler; }
    public ACharacter Character { get => _character; }
    public Joystick Joystick { get => _joystick; }

    #endregion

    private void Start()
    {
        TranslateSystem = GameObject.FindGameObjectWithTag(TRANSLATE_TAG).GetComponent<TranslateSystem>();
    }

    public void Load3C(CameraHandler cameraHandler, ACharacter character, VariableJoystick joystick)
    {
        _cameraHandler = cameraHandler;
        _character = character;
        _joystick = joystick;
    }
}
