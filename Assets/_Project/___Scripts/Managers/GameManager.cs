using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;


[DefaultExecutionOrder(-10)]


public class GameManager : Singleton<GameManager>
{
    private const string PLAYER_TAG = "Player";
    private const string SOUND_MANAGER_TAG = "SoundManager";
    private const string MAIN_CAMERA_TAG = "MainCamera";

    [HideInInspector] public GameObject MainCamera;
    [HideInInspector] public SoundSystem SoundSystem;

    private CameraHandler _cameraHandler;
    private ACharacter _character;
    private Joystick _joystick;

    public delegate void LoadManager();
    public event LoadManager OnLoadManager;

    #region Properties

    public CameraHandler CameraHandler { get => _cameraHandler;}
    public ACharacter Character { get => _character;}
    public Joystick Joystick { get => _joystick; }

    #endregion

    private void Start()
    {
        //MainCamera = GameObject.FindGameObjectWithTag(MAIN_CAMERA_TAG);
        //SoundSystem = GameObject.FindGameObjectWithTag(SOUND_MANAGER_TAG).GetComponent<SoundSystem>();
        //Ca marche pas wola

        OnLoadManager?.Invoke();

    }

    public void Load3C(CameraHandler cameraHandler, ACharacter character, Joystick Joystick)
    {
        _cameraHandler = cameraHandler;
        _character = character;
        _joystick = Joystick;
    }

}