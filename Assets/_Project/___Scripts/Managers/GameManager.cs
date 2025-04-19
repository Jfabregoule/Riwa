using System;
using UnityEngine;
using static Statue;


public enum EnumTemporality
{
    Past,
    Present
}

[DefaultExecutionOrder(-10)]
public class GameManager : Singleton<GameManager>
{
    private const string PLAYER_TAG = "Player";
    private const string SOUND_MANAGER_TAG = "SoundManager";
    private const string MAIN_CAMERA_TAG = "MainCamera";
    private const string TRANSLATE_TAG = "TranslateSystem";

    [HideInInspector] public GameObject MainCamera;
    [HideInInspector] public RiwaSoundSystem SoundSystem;
    [HideInInspector] public TranslateSystem TranslateSystem;

    private CameraHandler _cameraHandler;
    private ACharacter _character;
    private VariableJoystick _joystick;
    private EnumTemporality _currentTemporality;

    public delegate void ChangeTimeEvent(EnumTemporality temporality);
    public event ChangeTimeEvent OnTimeChangeStarted;
    public event ChangeTimeEvent OnTimeChangeEnded;
    public event Action OnTimeChangeAborted;

    #region Properties

    public CameraHandler CameraHandler { get => _cameraHandler; }
    public ACharacter Character { get => _character; }
    public Joystick Joystick { get => _joystick; }
    public EnumTemporality CurrentTemporality { get => _currentTemporality; set => _currentTemporality = value; }

    #endregion

    private void Start()
    {
        TranslateSystem = GameObject.FindGameObjectWithTag(TRANSLATE_TAG).GetComponent<TranslateSystem>();
        CurrentTemporality = EnumTemporality.Present;
    }

    public void Load3C(CameraHandler cameraHandler, ACharacter character, VariableJoystick joystick)
    {
        _cameraHandler = cameraHandler;
        _character = character;
        _joystick = joystick;
    }

    public void TimeChangeStarted()
    {
        if (_currentTemporality == EnumTemporality.Present)
        {
            _currentTemporality = EnumTemporality.Past;
        }
        else if (_currentTemporality == EnumTemporality.Past)
        {
            _currentTemporality = EnumTemporality.Present;
        }

        OnTimeChangeStarted?.Invoke(_currentTemporality);
    }

    public void TimeChangeEnded()
    {
        OnTimeChangeEnded?.Invoke(_currentTemporality);
    }

    public void TimeChangeAborted()
    {
        OnTimeChangeEnded?.Invoke(_currentTemporality);
    }
}
