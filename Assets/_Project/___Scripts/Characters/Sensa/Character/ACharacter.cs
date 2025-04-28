using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ACharacter : APawn<EnumStateCharacter>, IRespawnable
{
    #region Constantes

    //Constantes

    public const string PAWN_OBJECT = "Pawn";
    public const string SOUL_OBJECT = "Soul";
    public const string CAMERA_TARGET_OBJECT = "CameraTarget";
    public const string CAMERA_TARGET_PARENT_OBJECT = "CameraTargetParent";

    #endregion

    #region Fields

    //Field

    private GameObject _pawn;
    //new private StateMachineCharacter _stateMachine;
    private GameObject _soul;
    private GameObject _holdingObject;

    private bool _canInteract;
    private bool _canInteractSoul;
    private bool _canChangeTime = true;

    private ChangeTime _changeTime;
    private CharacterFeet _feet;

    private CameraHandler _cameraHandler;

    [Header("Gameplay values")]

    [SerializeField] private float _joystickRunTreshold = 0.4f;
    [SerializeField] private Vector3 _respawnPosition;
    [SerializeField] private Vector3 _respawnRotation;

    [Header("StateMachine values")]

    [SerializeField] private float _timeBeforeWait = 15f;
    [SerializeField] private bool _isChangingTime = false;
    [SerializeField] private bool _isInSoul = false;
    [SerializeField] private float _pushingSpeed = 1;
    [SerializeField] private float _fallingStun = 0.5f;

    //Animation values

    private float magnitudeVelocity;

    public delegate void HoldingEvent();
    public event HoldingEvent OnHoldingStart;
    public event HoldingEvent OnHoldingEnd;
    //
    public event IRespawnable.RespawnEvent OnRespawn;
    public event System.Action OnRotate;
    public event System.Action OnInteractAnimation;

    public event System.Action OnFinishAnimationSpeak;
    private bool _canSkipAnimationSpeak;

    #endregion

    #region Properties

    //Properties

    public GameObject Pawn { get => _pawn; }
    public GameObject HoldingObject { get => _holdingObject; }
    public bool CanInteract { get => _canInteract; set => _canInteract = value; }
    public bool CanInteractSoul { get => _canInteractSoul; set => _canInteractSoul = value; }
    public float JoystickRunTreshold { get => _joystickRunTreshold; set => _joystickRunTreshold = value; }
    public float TimeBeforeWait { get => _timeBeforeWait; set => _timeBeforeWait = value; }
    public bool IsChangingTime { get => _isChangingTime; set => _isChangingTime = value; }

    public float PushingSpeed { get => _pushingSpeed; }
    public ChangeTime ChangeTime { get => _changeTime; }
    public bool IsInSoul { get => _isInSoul; set => _isInSoul = value; }
    public GameObject Soul { get => _soul; set => _soul = value; }
    public CameraHandler CameraHandler { get => _cameraHandler; }
    new public StateMachineCharacter StateMachine { get => (StateMachineCharacter)_stateMachine; set => _stateMachine = value; }
    public bool CanChangeTime { get => _canChangeTime; set => _canChangeTime = value; }
    public Vector3 RespawnPosition { get => _respawnPosition; set => _respawnPosition = value; }
    public Vector3 RespawnRotation { get => _respawnRotation; set => _respawnRotation = value; }
    public float MagnitudeVelocity { get => magnitudeVelocity; set => magnitudeVelocity = value; }
    public CharacterFeet Feet { get => _feet; set => _feet = value; }

    #endregion

    #region Methods

    //Methods

    public void Awake()
    {
        _pawn = GameObject.Find(PAWN_OBJECT);
        _rb = GetComponent<Rigidbody>();
        _changeTime = GetComponent<ChangeTime>();
        _inputManager = InputManager.Instance;
        _animator = GetComponent<Animator>();
        _feet = GetComponentInChildren<CharacterFeet>();

        _soul = GameObject.Find(SOUL_OBJECT);
        _capsuleCollider = GetComponent<CapsuleCollider>();

        StateMachine = new StateMachineCharacter();
    }

    public void Start()
    {
        _cameraHandler = GameManager.Instance.CameraHandler; //Il faut appeler ça après le load des 3C dans gameManager

        StateMachine.InitStateMachine(this);
        StateMachine.InitState(_stateMachine.States[EnumStateCharacter.Idle]);
    }

    private void Update()
    {
        StateMachine.StateMachineUpdate();

    }

    private void FixedUpdate()
    {
        StateMachine.StateMachineFixedUpdate();

        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    public void SetHoldingObject(GameObject holdingObject)
    {
        _holdingObject = holdingObject;
    }

    public void TriggerChangeTempo()
    {
        if (!_canChangeTime) { return; }
        StateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.ChangeTempo]);
    }

    public void Respawn()
    {
        OnRespawn?.Invoke();
        StateMachine.ChangeState(_stateMachine.States[EnumStateCharacter.Respawn]);
    }

    public void InvokeFallStun()
    {
        FallStateCharacter state = (FallStateCharacter)StateMachine.States[EnumStateCharacter.Fall];
        StartCoroutine(state.FallStun(_fallingStun));
    }

    public void InvokeRotate()
    {
        OnRotate?.Invoke();
    }

    public void InvokeInteract()
    {
        OnInteractAnimation?.Invoke();
    }

    private void LoadCharacterData()
    {
        if (SaveSystem.Instance.ContainsElements("RespawnPosition"))
            RespawnPosition = SaveSystem.Instance.LoadElement<SerializableVector3>("RespawnPosition").ToVector3();
        if (SaveSystem.Instance.ContainsElements("RespawnRotation"))
            RespawnRotation = SaveSystem.Instance.LoadElement<SerializableVector3>("RespawnRotation").ToVector3();
    }

    private void SaveCharacterData()
    {
        SerializableVector3 respawnPosition = new SerializableVector3(RespawnPosition);
        SaveSystem.Instance.SaveElement<SerializableVector3>("RespawnPosition", respawnPosition);
        SerializableVector3 respawnRotation = new SerializableVector3(RespawnRotation);
        SaveSystem.Instance.SaveElement<SerializableVector3>("RespawnRotation", respawnRotation);
    }

    public void InvokeHoldingStart()
    {
        OnHoldingStart?.Invoke();
    }
    public void InvokeHoldingEnd()
    {
        OnHoldingEnd?.Invoke();
    }
    public void InvokeCinematicSpeak()
    {
        OnFinishAnimationSpeak?.Invoke();
    }

    //Pour trigger animation Sensa

    public void LaunchSensaSpeakingAnimation()
    {
        StartCoroutine(SensaSpeakingCoroutine());
    }

    private IEnumerator SensaSpeakingCoroutine()
    {
        _canSkipAnimationSpeak = false;

        OnFinishAnimationSpeak += SetCanSkip;
        Animator.SetTrigger("CinematicSpeak");

        while (_canSkipAnimationSpeak)
        {
            yield return null;
        }

        OnFinishAnimationSpeak -= SetCanSkip;

    }

    public void SetCanSkip()
    {
        _canSkipAnimationSpeak = true;
    }

    #endregion

}
