using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ACharacter : APawn<EnumStateCharacter>, IRespawnable
{
    #region Constantes

    //Constantes

    public const string PAWN_OBJECT = "Pawn";
    public const string SOUL_OBJECT = "Soul";
    public const string CAMERA_TARGET_OBJECT= "CameraTarget";
    public const string CAMERA_TARGET_PARENT_OBJECT= "CameraTargetParent";

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

    [Header("VFX")]

    [SerializeField] private ParticleSystem _soulLinkVFX;

    //Animation values

    private float magnitudeVelocity;

    //

    public event IRespawnable.RespawnEvent OnRespawn;
    public event System.Action OnRotate;

    #endregion

    #region Properties

    //Properties

    public GameObject Pawn { get => _pawn;}
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
    public ParticleSystem SoulLinkVFX { get => _soulLinkVFX; set => _soulLinkVFX = value; }
    public CameraHandler CameraHandler { get => _cameraHandler;}
    new public StateMachineCharacter StateMachine { get => (StateMachineCharacter)_stateMachine; set => _stateMachine = value; }
    public bool CanChangeTime { get => _canChangeTime; set => _canChangeTime = value; }
    public Vector3 RespawnPosition { get => _respawnPosition; set => _respawnPosition = value; }
    public Vector3 RespawnRotation { get => _respawnRotation; set => _respawnRotation = value; }
    public float MagnitudeVelocity { get => magnitudeVelocity; set => magnitudeVelocity = value; }
    public CharacterFeet Feet { get => _feet; set => _feet = value; }

    #endregion

    #region Methods

    //Methods

    public void OnEnable()
    {
        _pawn = GameObject.Find(PAWN_OBJECT);
        _rb = GetComponent<Rigidbody>();
        _changeTime = GetComponent<ChangeTime>();
        _inputManager = InputManager.Instance;
        _animator = GetComponent<Animator>();
        _feet = GetComponentInChildren<CharacterFeet>();

        _soul = GameObject.Find(SOUL_OBJECT);
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _soul.SetActive(false);

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

    #endregion

}
