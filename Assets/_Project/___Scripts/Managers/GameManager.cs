using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
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
    private const string COLLECTIBLE_TAG = "CollectibleManager";
    private const string UIMANAGER_TAG = "UIManager";

    [HideInInspector] public GameObject MainCamera;
    [HideInInspector] public RiwaSoundSystem SoundSystem;
    [HideInInspector] public TranslateSystem TranslateSystem;
    [HideInInspector] public CollectibleManager CollectibleManager;
    [HideInInspector] public UIManager UIManager;


    private CameraHandler _cameraHandler;
    private ACharacter _character;
    private VariableJoystick _joystick;
    private EnumTemporality _currentTemporality;
    private BaseLevelManager _currentLevelManager;

    public delegate void ShowInput();
    public event ShowInput OnShowBasicInputEvent;
    public event ShowInput OnShowMoveInputEvent;
    public event ShowInput OnUnlockChangeTime;

    public delegate void ChangeTimeEvent(EnumTemporality temporality);
    public event ChangeTimeEvent OnTimeChangeStarted;
    public event ChangeTimeEvent OnTimeChangeEnded;
    public event ChangeTimeEvent OnTimeChangeAborted;

    public delegate void RoomChangeEvent();
    public event RoomChangeEvent OnRoomChange;
    public event RoomChangeEvent OnCredit;
    public event RoomChangeEvent OnResetSave;

    #region Properties

    public CameraHandler CameraHandler { get => _cameraHandler; }
    public ACharacter Character { get => _character; }
    public Joystick Joystick { get => _joystick; }
    public EnumTemporality CurrentTemporality { get => _currentTemporality; set => _currentTemporality = value; }
    public BaseLevelManager CurrentLevelManager { get => _currentLevelManager; set => _currentLevelManager = value; }
    public bool ChangeTimeUnlock { get; set; }

    #endregion

    private void Start()
    {
        CollectibleManager = GameObject.FindGameObjectWithTag(COLLECTIBLE_TAG).GetComponent<CollectibleManager>();
        TranslateSystem = GameObject.FindGameObjectWithTag(TRANSLATE_TAG).GetComponent<TranslateSystem>();
        //UIManager = GameObject.FindGameObjectWithTag(UIMANAGER_TAG).GetComponent<UIManager>();
        StartCoroutine(Helpers.WaitMonoBeheviour(() => GameObject.FindGameObjectWithTag(UIMANAGER_TAG), WaitUIManager));
        CurrentTemporality = EnumTemporality.Present;
        ChangeTimeUnlock = false;
    }

    public void Load3C(CameraHandler cameraHandler, ACharacter character, VariableJoystick joystick)
    {
        _cameraHandler = cameraHandler;
        _character = character;
        _joystick = joystick;
        OnRoomChange?.Invoke();
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
        OnTimeChangeAborted?.Invoke(_currentTemporality);
    }

    public void UnlockChangeTime()
    {
        ChangeTimeUnlock = true;
        OnUnlockChangeTime?.Invoke();
    }


    public void InvokeBasicInput()
    {
        OnShowBasicInputEvent?.Invoke();
    }
    public void InvokeInteractInput()
    {
        OnShowMoveInputEvent?.Invoke();
    }

    public void InvokeCredit()
    {
        OnCredit?.Invoke();
    }

    public void ResetSave()
    {
        CurrentTemporality = EnumTemporality.Present;
        ChangeTimeUnlock = false;
        OnResetSave?.Invoke();
    }

    private void WaitUIManager(GameObject go)
    {
        if (go != null)
        {
            UIManager = go.GetComponent<UIManager>();
        }
    }

    private void OnDisable()
    {
    }
}
