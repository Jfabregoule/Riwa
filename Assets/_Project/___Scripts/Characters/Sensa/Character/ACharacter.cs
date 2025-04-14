using TMPro;
using UnityEngine;

public class ACharacter : APawn<EnumStateCharacter>
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
    new private StateMachineCharacter _stateMachine;
    private Animator _animator;
    private GameObject _soul;
    private GameObject _holdingObject;

    private bool _canInteract;
    private bool _canInteractSoul;

    private ChangeTime _changeTime;

    private CameraHandler _cameraHandler;

    [Header("Gameplay Statistics")]

    [SerializeField] private float _joystickRunTreshold = 0.4f;

    [Header("StateMachine values")]

    [SerializeField] private float _timeBeforeWait = 15f;
    [SerializeField] private bool _isChangingTime = false;
    [SerializeField] private bool _isInSoul = false;
    [SerializeField] private float _pushingSpeed = 1;

    [Header("VFX")]

    [SerializeField] private ParticleSystem _soulLinkVFX;

    #endregion

    #region Properties

    //Properties

    public GameObject Pawn { get => _pawn;}
    public Animator Animator { get => _animator;}

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
    new public StateMachineCharacter StateMachine { get => _stateMachine; set => _stateMachine = value; }

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

        _soul = GameObject.Find(SOUL_OBJECT);
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _soul.SetActive(false);

        _stateMachine = new StateMachineCharacter();
        _stateMachine.InitStateMachine(this);
        _stateMachine.InitState(_stateMachine.States[EnumStateCharacter.Idle]);
    }

    public void Start()
    {
        _cameraHandler = GameManager.Instance.CameraHandler; //Il faut appeler ça après le load des 3C dans gameManager
        Application.targetFrameRate = 300;
    }

    private void Update()
    {
        _stateMachine.StateMachineUpdate();
    }

    private void FixedUpdate()
    {
        _stateMachine.StateMachineFixedUpdate();
    }

    public void SetHoldingObject(GameObject holdingObject)
    {
        _holdingObject = holdingObject;
    }

    #endregion

}
